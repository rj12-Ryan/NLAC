using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ILGPU;
using ILGPU.Runtime;

namespace NLAC
{
    //CUDA normalization for NTables
    public partial class NTable
    {
        public void CUDANormalize(NColumn c,int lutSize)
        {
            double ColMax = c.InputValues.Where(d => !double.IsNaN(d)).DefaultIfEmpty(double.MinValue).Max();
            double ColMin = c.InputValues.Where(d => !double.IsNaN(d)).DefaultIfEmpty(double.MaxValue).Min();

            // Initialize ILGPU.
            Context context = Context.CreateDefault();
            Accelerator accelerator = context.GetPreferredDevice(preferCPU: false)
                                      .CreateAccelerator(context);

            // Load the data.
            MemoryBuffer1D<double, Stride1D.Dense> deviceData = accelerator.Allocate1D(c.InputValues.ToArray());
            MemoryBuffer1D<int, Stride1D.Dense> deviceOutput = accelerator.Allocate1D<int>(c.InputValues.Count);

            // load / precompile the kernel
            Action<Index1D, ArrayView<double>, ArrayView<int>, double, double, int> loadedKernel =
                accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView<double>, ArrayView<int>, double, double, int>(Kernel);

            // finish compiling and tell the accelerator to start computing the kernel
            loadedKernel((int)deviceOutput.Length, deviceData.View, deviceOutput.View, ColMax, ColMin, lutSize);

            // wait for the accelerator to be finished with whatever it's doing
            // in this case it just waits for the kernel to finish.
            accelerator.Synchronize();

            // moved output data from the GPU to the CPU for output to console
            int[] hostOutput = deviceOutput.GetAsArray1D();

            for (int i = 0; i < c.InputValues.Count; i++)
            {
                c.NormalValues.Add(hostOutput[i]);
            }

            accelerator.Dispose();
            context.Dispose();
        }

        //Define a CUDA Kernel to perform distributed column-wise normaliztion
        static void Kernel(Index1D i, ArrayView<double> data, ArrayView<int> output, double colMax, double colMin, int lutSize)
        {
            if (Double.IsNaN(data[i]) && data[i] != 0)
            {
                output[i] = -1;
            }
            else
            {
                output[i] = (int)((data[i] - colMin) / (colMax - colMin) * (lutSize - 1));
            }
        }

    }
}
