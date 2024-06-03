using ILGPU.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NLAC
{
    
    internal static class NMXML
    {
        private static scorepartwise score = new scorepartwise();

        public static void genXML(OptionStager o)
        {
            //Upper header
            score.work = new work();
            score.work.worktitle = $"NLAC Track - {o.Title}";

            score.identification = new identification();
            {
                typedtext tt = new typedtext();
                tt.type = "composer";
                tt.Value = $"{o.Composer}";
                typedtext[] tta = { tt };
                score.identification.creator = tta;
            }

            
            //Parts list
            partlist pl = new partlist();
            pl.Items = new scorepart[o.Table.Columns.Count];
            {
                foreach(NColumn col in o.Table.Columns)
                {
                    int index = o.Table.Columns.IndexOf(col);
                    scorepart sp = new scorepart();
                    {
                        sp.id = $"P{index + 1}";
                        sp.partname = new partname();
                        {
                            sp.partname.Value = col.Name;
                        }
                        sp.scoreinstrument = new scoreinstrument[1];
                        {
                            sp.scoreinstrument[0] = new scoreinstrument();
                            sp.scoreinstrument[0].id = $"I{index+1}-{index+1}";
                            sp.scoreinstrument[0].instrumentname = col.Name;
                        }
                        sp.midiinstrument = new midiinstrument[1];
                        {
                            sp.midiinstrument[0] = new midiinstrument();
                            sp.midiinstrument[0].id = $"I{index + 1}-{index + 1}";
                            //This might need to be a +1 on the instrument enum
                            sp.midiinstrument[0].midiprogram = ((int)col.Instrument).ToString();
                        }
                        
                    }
                    pl.Items[index] = sp;
                }   
                score.partlist = pl;
            }

            //Parts
            score.part = new scorepartwisePart[o.Table.Columns.Count];
            //create a new part for each NColumn
            foreach(NColumn col in o.Table.Columns)
            {
                int index = o.Table.Columns.IndexOf(col);

                scorepartwisePart part = new scorepartwisePart();
                part.id = $"P{index+1}";

                part.measure = new scorepartwisePartMeasure[col.LookupValues.Count];
                {
                    int measureIndex = 0;
                    foreach (string abcNote in col.LookupValues)
                    {
                        //part.measure[measureIndex] = buildMeasureFromABC(abcNote, measureIndex, o);
                        measureIndex++;
                    }
                }

                score.part[index] = part;
            }
            


            XmlSerializer xml = new XmlSerializer(typeof(scorepartwise));
            TextWriter writer = File.CreateText("test.xml");
            xml.Serialize(writer, score);

        }
    
        //build a new measure from an abc note
       private static scorepartwisePartMeasure buildMeasureFromABC(string abcNote, int measureIndex, OptionStager o)
       {
            scorepartwisePartMeasure measure = new scorepartwisePartMeasure();
            measure.Items = new object[2];
            measure.number = $"{measureIndex + 1}";
            int measureItemCount = 0;

            //add additional header information to the first measure block
            if(measureIndex == 0)
            {
                measure.Items = new object[3];
                direction direction = new direction();
                {
                    direction.placement = abovebelow.above;

                    direction.directiontype = new directiontype[1] { new directiontype() };
                    direction.directiontype[0].Items = new object[1];
                    directiontype dtype = new directiontype();
                    {
                        metronome metronome = new metronome();
                        {
                            beatunittied beatunittied = new beatunittied();
                            beatunittied.beatunit = notetypevalue.quarter;
                            perminute perminute = new perminute();
                            perminute.Value = o.Tempo.ToString();

                            metronome.Items = new object[2];
                            metronome.Items[0]=(beatunittied); metronome.Items[1]=(perminute);

                            direction.directiontype[0].Items[0] = (metronome);
                        }
                    }

                    direction.sound = new sound();
                    direction.sound.tempo = o.Tempo;
                }
                measure.Items[measureItemCount]=(direction);
                measureItemCount++;

                attributes at = new attributes();
                {
                    at.divisions = 120;
                    at.key = new key[1];
                    {
                        at.key[0] = new key();
                       
                        at.key[0].number = "3";
                        at.key[0].Items = new object[1];
                        numeralkey nk = new numeralkey();
                        nk.numeralfifths = "3";

                        at.key[0].Items[0] = nk.numeralfifths;

                        
                    }
                    

                }
            }
            //add just the regular note data to all measure blocks

            note note = new note();


            return measure;
       }
    }
}
