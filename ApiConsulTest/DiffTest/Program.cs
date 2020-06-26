using DiffMatchPatch;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffTest
{
    class Program
    {

        public static void TESTDIFF()
        {
            var dmp = new diff_match_patch();
            var data1 = "<p class=\"MsoNormal\" style=\"margin: 0px; font-size: 16px; font-family: &quot;Times New Roman&quot;, serif;\"><u><span            style=\"font-size: 17px; font-family: &quot;Cordia New&quot;, sans-serif;\">ประมาณทางการเงิน            ปี 2562-2567</span></u></p><table class=\"MsoNormalTable\" cellspacing=\"0\" cellpadding=\"0\" width=\"724\"    style=\"width: 721px; margin-left: 7px;\">    <tbody>        <tr style=\"height: 19px;\">            <td width=\"150\"  rowspan=\"2\" style=\"width: 150px; padding: 0px 7px; height: 19px;\">                <p class=\"MsoNormal\"                    style=\"margin: 0px; font-size: 16px; font-family: &quot;Times New Roman&quot;, serif;\"><b><span                            style=\"font-size: 12px; font-family: &quot;Cordia New&quot;, sans-serif;\">DESCRIPTION</span></b>                </p>            </td>            <td width=\"287\"  colspan=\"6\" valign=\"bottom\" style=\"width: 286px; padding: 0px 7px; height: 19px;\">                <p class=\"MsoNormal\"                    style=\"margin: 0px; font-size: 16px; font-family: &quot;Times New Roman&quot;, serif;\"><b><span                            style=\"font-size: 12px; font-family: &quot;Cordia New&quot;, sans-serif;\">Customer                            case</span></b></p>            </td>            <td width=\"287\"  colspan=\"6\" valign=\"bottom\"                style=\"width: 286px; background: rgb(221, 217, 195); padding: 0px 7px; height: 19px;\">                <p class=\"MsoNormal\"                    style=\"margin: 0px; font-size: 16px; font-family: &quot;Times New Roman&quot;, serif;\"><b><span                            style=\"font-size: 12px; font-family: &quot;Cordia New&quot;, sans-serif; color: black;\">Moderate                            case</span></b><b><span                            style=\"font-size: 12px; font-family: &quot;Cordia New&quot;, sans-serif;\"></span></b></p>            </td>        </tr>    </tbody></table> <b> ter </b>";
            var data2 = "<p class=\"MsoNormal\" style=\"margin: 0px; font-size: 16px; font-family: &quot;Times New Roman&quot;, serif;\"><u><span            style=\"font-size: 17px; font-family: &quot;Cordia New&quot;, sans-serif;\">ประมาณทางการเงิน            ปี 2562-2567</span></u></p> <b> test </b>";
            data1 = data1.Replace(@"\""", @"""");
            data2 = data2.Replace(@"\""", @"""");

            var diffs = dmp.diff_main(data1 , data2  );
            Console.WriteLine(string.Join("", diffs.Select(s => s.text).ToArray()));

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            var html = dmp.diff_prettyHtml_Modified(diffs);
            Console.WriteLine(html);
        }

        public static void diff_prettyHtmlTest()
        {
            diff_match_patch dmp = new diff_match_patch();
            // Pretty print.
            List<Diff> diffs = new List<Diff> {
              new Diff(Operation.EQUAL, "a\n"),
              new Diff(Operation.DELETE, "<B>b</B>"),
              new Diff(Operation.INSERT, "c&d")
            };
            var aaa = "<span>a&para;<br></span><del style=\"background:#ffe6e6;\">&lt;B&gt;b&lt;/B&gt;</del><ins style=\"background:#e6ffe6;\">c&amp;d</ins>";
            var bbb = dmp.diff_prettyHtml(diffs);
            Console.WriteLine(aaa);
            Console.WriteLine(bbb);

        }

        public static void Main(string[] args)
        {
            TESTDIFF();
            Console.ReadKey();
            return;
            diff_prettyHtmlTest();

            diff_match_patch dmp = new diff_match_patch();
            List<Diff> diff = dmp.diff_main("Hello orld. agendar", "Hello World. asd");
            // Result: [(-1, "Hell"), (1, "G"), (0, "o"), (1, "odbye"), (0, " World.")]
            dmp.diff_cleanupSemantic(diff);
            var fullText = "";
            // Result: [(-1, "Hello"), (1, "Goodbye"), (0, " World.")]

            var lastStage = Operation.EQUAL;
            for (int i = 0; i < diff.Count; i++)
            {
                var text = diff[i].text;
                var print = diff[i].operation == Operation.EQUAL;
                var currOperation = diff[i].operation;
                if (lastStage != currOperation && lastStage == Operation.EQUAL)
                {
                    fullText += "<i>";
                }
                else if (lastStage != currOperation && lastStage != Operation.EQUAL)
                {
                    fullText += "</i>";
                }
                if (print)
                {
                    fullText += text;

                    lastStage = diff[i].operation;
                }
                else
                {
                    fullText += text;

                    lastStage = diff[i].operation;
                }

            }

            if (lastStage != Operation.EQUAL)
            {
                fullText += "</i>";
            }

            Console.WriteLine(fullText);

            Console.ReadKey();
        }
    }
}
