using Microsoft.EntityFrameworkCore;
using NTE.DbContextENCRYPT;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Collections.Generic;

namespace FINDFIELD
{
    class Program
    {
        static void Main(string[] args)
        {
            var tableName = "CBMSAPP012";

            Console.WriteLine($"Table Name {tableName}");

            capdb context = new capdb();
            var procedureList = context.INFORMATIONSCHEMA.FromSqlRaw(@$"
            SELECT ROUTINE_NAME, ROUTINE_DEFINITION
            FROM INFORMATION_SCHEMA.ROUTINES
            WHERE ROUTINE_DEFINITION LIKE '%{tableName}%'
            --AND ROUTINE_TYPE = 'PROCEDURE'
"
            ).ToListAsync().Result;

            //procedureList = procedureList.Take(1).ToList();

            var procedureNames = procedureList.Where(w => !w.ROUTINE_NAME.Contains("usp_rp")).Select(s => s.ROUTINE_NAME).ToList();

            var ProcedureNameIsProblem = new List<string>();
            var IndexProblem = new List<string>();

            procedureNames.ForEach(fProcedureName =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"ProcedureName > {fProcedureName}");
                Console.ResetColor();

                var procudureString = context.SPHELPTEXT.FromSqlRaw($"sp_helptext'{fProcedureName}'").ToListAsync().Result;

                var declareLines = procudureString.Where(w => w.Text.ToLower().Contains(tableName.ToLower())).Select(s => s.Text).ToList();

                declareLines.ForEach(declareLine =>
                {
                    #region hide

                    declareLine = declareLine.TrimStart();
                    declareLine = declareLine.ToLower();
                    declareLine = declareLine.Replace("\t", " ");
                    declareLine = declareLine.Replace("\n", " ");
                    declareLine = declareLine.Replace(" AS ", " ");
                    declareLine = declareLine.Replace(" as ", " ");


                    var firstIndexOf = declareLine.IndexOf(tableName.ToLower());
                    var afterTable = declareLine.Substring(firstIndexOf + tableName.Length);
                    afterTable = afterTable.TrimStart();

                    var indexOfFirstSpaceAfterTable = afterTable.IndexOf(" ");
                    var varNameOfTable = "";
                    if (indexOfFirstSpaceAfterTable == -1)
                    {
                        IndexProblem.Add(fProcedureName + " >>> " + afterTable);
                        varNameOfTable = tableName;
                    }
                    else
                    {
                        varNameOfTable = afterTable.Substring(0, indexOfFirstSpaceAfterTable);
                    }
                    // fstNamTH ,surNamTH , faFstNamTH ,faSurNamTH , maFstNamTH ,maSurNamTH , moFstNamTH ,moSurNamTH

                    //var aliasName = new List<string> { 
                    //    varNameOfTable + ".fstNamTH",
                    //    varNameOfTable + ".surNamTH" ,
                    //    varNameOfTable + ".faFstNamTH",
                    //    varNameOfTable + ".faSurNamTH",
                    //    varNameOfTable + ".maFstNamTH",
                    //    varNameOfTable + ".maSurNamTH",
                    //    varNameOfTable + ".moFstNamTH",
                    //    varNameOfTable + ".moSurNamTH"
                    //};

                    //var aliasNameTest = new List<string> {
                    //    ".actorName",
                    //    ".actorSurName" 
                    //};
                    //var ContainAliasTest = procudureString.Where(procud => aliasNameTest.Any(name => procud.Text.ToLower().Replace(" ","").Contains(name.ToLower()))).ToList();
                    //if (ContainAliasTest.Any())
                    //{
                    //    ContainAliasTest.ForEach(conAlias =>
                    //    {
                    //        ProcedureNameIsProblem.Add(fProcedureName + "   =|>  " + conAlias.Text);
                    //    });

                    //}

                    #endregion
                    var aliasName = new List<string> {
                         varNameOfTable + ".payAmt", 
                    };
                      //aliasName = new List<string> {"update" };
                    //var aliasName = new List<string> { varNameOfTable + ".brnCde", varNameOfTable + ".accTypCde" };
                    //var ContainAlias = procudureString.Where(procud => aliasName.Any(name => procud.Text.ToLower().Contains(name.ToLower()))).ToList();

                    var ContainAlias = procudureString.Where(procud => aliasName.Any(name => procud.Text.ToLower().Contains(name.ToLower()))).ToList();
                    procudureString.ForEach(procud =>
                    {
                        if (aliasName.Any(name => procud.Text.ToLower().Contains(name.ToLower())))
                        {
                            var dataProcud = procud.Text;
                            Console.ForegroundColor = ConsoleColor.Red;
                            ContainAlias.ForEach(conAlias =>
                            {
                                Console.WriteLine("conAlias  " + dataProcud);
                            });
                            ProcedureNameIsProblem.Add(fProcedureName + "   =|>  " + dataProcud);
                        }
                    });


                    //if (ContainAlias.Any())
                    //{
                    //    Console.ForegroundColor = ConsoleColor.Red;
                    //    ContainAlias.ForEach(conAlias =>
                    //    {
                    //        Console.WriteLine("conAlias  " + conAlias.Text);
                    //    });
                    //    Console.ResetColor();
                    //    ProcedureNameIsProblem.Add(fProcedureName);
                    //}


                    Console.WriteLine(declareLine + " :> " + afterTable);
                    Console.WriteLine("varNameOfTable  " + varNameOfTable);
                });




            });

            IndexProblem.ForEach(f =>
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("index problem : " + f);
                Console.ResetColor();
            });

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("-------------------- ");
            Console.ResetColor();
            ProcedureNameIsProblem.Distinct().ToList().ForEach(f =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("stored name is problem : " + f);
                Console.ResetColor();
            });

            Console.WriteLine("end");
            Console.ReadKey();
        }
    }
}
