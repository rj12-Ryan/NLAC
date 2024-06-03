using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace NLAC
{
    //Standard CPU normalization for an NTable
    public partial class NTable
    {
        private int _lutSize;
        public void Normalize(LUT lut)
        {
            //define a local LUTSize
            _lutSize = lut.Size();

            //If CUDA processing enabled, pass along required data to that
            //get OptionStager instance from the singleton
            OptionStager o = OptionStager.GetInstance();
            if (o.CUDA)
            {
                //Process each column separetly using the CUDA implentation (probably slow)
                Parallel.ForEach(this.Columns, c =>
                {
                    CUDANormalize(c, _lutSize);
                });
            }
            else
            {
                //multithreaded iteration through each column
                Parallel.ForEach(this.Columns, c =>
                {
                    _threadedColumnNormalizer(c);
                });

                //if in verbose output table
                if (o.Verbose)
                {
                    Output.NTable(this, NColumn.Type.Normal, "Normalized Data");
                }
            }
            
        }

        //main normalization code
        private void _threadedColumnNormalizer(NColumn c)
        { 
            double ColMax = c.InputValues.Where(d => !double.IsNaN(d)).DefaultIfEmpty(double.MinValue).Max();
            double ColMin = c.InputValues.Where(d => !double.IsNaN(d)).DefaultIfEmpty(double.MaxValue).Min();

			foreach (double val in c.InputValues)
			{

				if (Double.IsNaN(val) && val != 0)
				{
					c.NormalValues.Add(-1);
				}
				else
				{
					c.NormalValues.Add(((int)(Math.Round((double)(val - ColMin) / (ColMax - ColMin) * (this._lutSize - 1), 0))));
				}
			}
		}
    }
}
