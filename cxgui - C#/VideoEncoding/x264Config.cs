namespace Cxgui.VideoEncoding
{
    using Cxgui.External;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Forms;
    using System.ComponentModel;

    [Serializable]
    public class x264Config : VideoEncConfigBase
    {
        protected int _currentPass = 1;
        protected OrderedDictionary<string, x264ConfigNode> _optionDict = new OrderedDictionary<string, x264ConfigNode>();
        protected Dictionary<string, object> _presets;
        protected int _totalPass = 1;

        public x264Config()
        {
            int index = 0;
            object[][] objArrayArray1 = new object[0x4f][];
            object[] objArray1 = new object[4];
            objArray1[0] = "profile";
            objArray1[1] = 1;
            objArray1[2] = 0;
            string[] textArray1 = new string[4];
            textArray1[1] = "baseline";
            textArray1[2] = "main";
            textArray1[3] = "high";
            objArray1[3] = textArray1;
            objArrayArray1[0] = objArray1;
            object[] objArray2 = new object[4];
            objArray2[0] = "level";
            objArray2[1] = 1;
            objArray2[2] = 0;
            string[] textArray2 = new string[0x10];
            textArray2[1] = "1";
            textArray2[2] = "1.1";
            textArray2[3] = "1.2";
            textArray2[4] = "1.3";
            textArray2[5] = "2";
            textArray2[6] = "2.1";
            textArray2[7] = "2.2";
            textArray2[8] = "3";
            textArray2[9] = "3.1";
            textArray2[10] = "3.2";
            textArray2[11] = "4";
            textArray2[12] = "4.1";
            textArray2[13] = "4.2";
            textArray2[14] = "5";
            textArray2[15] = "5.1";
            objArray2[3] = textArray2;
            objArrayArray1[1] = objArray2;
            object[] objArray3 = new object[4];
            objArray3[0] = "preset";
            objArray3[1] = 1;
            objArray3[2] = 5;
            string[] textArray3 = new string[] { "ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow", "placebo" };
            objArray3[3] = textArray3;
            objArrayArray1[2] = objArray3;
            object[] objArray4 = new object[4];
            objArray4[0] = "tune";
            objArray4[1] = 1;
            objArray4[2] = 0;
            string[] textArray4 = new string[10];
            textArray4[1] = "film";
            textArray4[2] = "animation";
            textArray4[3] = "grain";
            textArray4[4] = "stillimage";
            textArray4[5] = "psnr";
            textArray4[6] = "ssim";
            textArray4[7] = "fastdecode";
            textArray4[8] = "zerolatency";
            textArray4[9] = "touhou";
            objArray4[3] = textArray4;
            objArrayArray1[3] = objArray4;
            objArrayArray1[4] = new object[] { "qp", 0, 0, 1, 0x40 };
            objArrayArray1[5] = new object[] { "crf", 0, 0x17, 0.1, 64.0 };
            objArrayArray1[6] = new object[] { "bitrate", 0, 0, 1, 0x186a0 };
            objArrayArray1[7] = new object[] { "pass", 0, 0, 1, 3 };
            objArrayArray1[8] = new object[] { "slow-firstpass", 2, false };
            objArrayArray1[9] = new object[] { "keyint", 0, 250, 0, 0x3e7 };
            objArrayArray1[10] = new object[] { "min-keyint", 0, 0x19, 0, 100 };
            objArrayArray1[11] = new object[] { "no-scenecut", 2, false };
            objArrayArray1[12] = new object[] { "scenecut", 0, 40, 0, 100 };
            objArrayArray1[13] = new object[] { "intra-refresh", 2, false };
            objArrayArray1[14] = new object[] { "bframes", 0, 3, 0, 0x10 };
            objArrayArray1[15] = new object[] { "b-adapt", 0, 1, 0, 2 };
            objArrayArray1[0x10] = new object[] { "b-bias", 0, 0, -100, 100 };
            object[] objArray18 = new object[4];
            objArray18[0] = "b-pyramid";
            objArray18[1] = 1;
            objArray18[2] = 2;
            string[] textArray5 = new string[] { "none", "strict", "normal" };
            objArray18[3] = textArray5;
            objArrayArray1[0x11] = objArray18;
            objArrayArray1[0x12] = new object[] { "no-cabac", 2, false };
            objArrayArray1[0x13] = new object[] { "ref", 0, 3, 0, 0x10 };
            objArrayArray1[20] = new object[] { "no-deblock", 2, false };
            objArrayArray1[0x15] = new object[] { "deblock1", 0, 0, -6, 6 };
            objArrayArray1[0x16] = new object[] { "deblock2", 0, 0, -6, 6 };
            objArrayArray1[0x17] = new object[] { "slices", 0, 0, 0, 100 };
            objArrayArray1[0x18] = new object[] { "slice-max-size", 0, 0, 0, 250 };
            objArrayArray1[0x19] = new object[] { "slice-max-mbs", 0, 0, 0, 100 };
            objArrayArray1[0x1a] = new object[] { "tff", 2, false };
            objArrayArray1[0x1b] = new object[] { "bff", 2, false };
            objArrayArray1[0x1c] = new object[] { "constrained-intra", 2, false };
            objArrayArray1[0x1d] = new object[] { "rc-lookahead", 0, 40, 0, 250 };
            objArrayArray1[30] = new object[] { "vbv-maxrate", 0, 0, 0, 0x186a0 };
            objArrayArray1[0x1f] = new object[] { "vbv-bufsize", 0, 0, 0, 0x186a0 };
            objArrayArray1[0x20] = new object[] { "vbv-init", 0, 0.9, 0, 1.0 };
            objArrayArray1[0x21] = new object[] { "crf-max", 0, 0, 0.1, 64.0 };
            objArrayArray1[0x22] = new object[] { "qpmin", 0, 10, 1, 0x33 };
            objArrayArray1[0x23] = new object[] { "qpmax", 0, 0x33, 1, 0x33 };
            objArrayArray1[0x24] = new object[] { "qpstep", 0, 4, 1, 0x33 };
            objArrayArray1[0x25] = new object[] { "ratetol", 0, 1.0, 0.01, 100.0 };
            objArrayArray1[0x26] = new object[] { "ipratio", 0, 1.4, 1.0, 10.0 };
            objArrayArray1[0x27] = new object[] { "pbratio", 0, 1.3, 1.0, 10.0 };
            objArrayArray1[40] = new object[] { "chroma-qp-offset", 0, 0, -12, 12 };
            objArrayArray1[0x29] = new object[] { "aq-mode", 0, 1, 0, 2 };
            objArrayArray1[0x2a] = new object[] { "aq-strength", 0, 1.0, 0, 2.0 };
            objArrayArray1[0x2b] = new object[] { "stats", 3, "x264_2pass.log" };
            objArrayArray1[0x2c] = new object[] { "no-mbtree", 2, false };
            objArrayArray1[0x2d] = new object[] { "qcomp", 0, 0.6, 0, 1.0 };
            objArrayArray1[0x2e] = new object[] { "cplxblur", 0, 20.0, 0, 999.0 };
            objArrayArray1[0x2f] = new object[] { "qblur", 0, 0.5, 0, 99.0 };
            objArrayArray1[0x30] = new object[] { "qpfile", 3, string.Empty };
            object[] objArray50 = new object[4];
            objArray50[0] = "partitions";
            objArray50[1] = 1;
            objArray50[2] = 3;
            string[] textArray6 = new string[] { "All", "None", "Custom", "Default" };
            objArray50[3] = textArray6;
            objArrayArray1[0x31] = objArray50;
            objArrayArray1[50] = new object[] { "p8x8", 2, true };
            objArrayArray1[0x33] = new object[] { "p4x4", 2, false };
            objArrayArray1[0x34] = new object[] { "b8x8", 2, true };
            objArrayArray1[0x35] = new object[] { "i8x8", 2, true };
            objArrayArray1[0x36] = new object[] { "i4x4", 2, true };
            object[] objArray56 = new object[4];
            objArray56[0] = "direct";
            objArray56[1] = 1;
            objArray56[2] = 1;
            string[] textArray7 = new string[] { "none", "spatial", "temporal", "auto" };
            objArray56[3] = textArray7;
            objArrayArray1[0x37] = objArray56;
            objArrayArray1[0x38] = new object[] { "no-weightb", 2, false };
            objArrayArray1[0x39] = new object[] { "weightp", 0, 2, 0, 2 };
            object[] objArray59 = new object[4];
            objArray59[0] = "me";
            objArray59[1] = 1;
            objArray59[2] = 1;
            string[] textArray8 = new string[] { "dia", "hex", "umh", "esa", "tesa" };
            objArray59[3] = textArray8;
            objArrayArray1[0x3a] = objArray59;
            objArrayArray1[0x3b] = new object[] { "merange", 0, 0x10, 4, 0x40 };
            objArrayArray1[60] = new object[] { "subme", 0, 7, 0, 10 };
            objArrayArray1[0x3d] = new object[] { "psy-rd1", 0, 1, 0, 10.0 };
            objArrayArray1[0x3e] = new object[] { "psy-rd2", 0, 0, 0, 10.0 };
            objArrayArray1[0x3f] = new object[] { "no-psy", 2, false };
            objArrayArray1[0x40] = new object[] { "no-mixed-refs", 2, false };
            objArrayArray1[0x41] = new object[] { "no-chroma-me", 2, false };
            objArrayArray1[0x42] = new object[] { "no-8x8dct", 2, false };
            objArrayArray1[0x43] = new object[] { "trellis", 0, 1, 0, 2 };
            objArrayArray1[0x44] = new object[] { "no-fast-pskip", 2, false };
            objArrayArray1[0x45] = new object[] { "no-dct-decimate", 2, false };
            objArrayArray1[70] = new object[] { "nr", 0, 0, 0, 0x2710 };
            objArrayArray1[0x47] = new object[] { "deadzone-inter", 0, 0x15, 0, 0x20 };
            objArrayArray1[0x48] = new object[] { "deadzone-intra", 0, 11, 0, 0x20 };
            object[] objArray74 = new object[4];
            objArray74[0] = "cqm";
            objArray74[1] = 1;
            objArray74[2] = 1;
            string[] textArray9 = new string[] { "jvt", "flat" };
            objArray74[3] = textArray9;
            objArrayArray1[0x49] = objArray74;
            objArrayArray1[0x4a] = new object[] { "cqmfile", 3, string.Empty };
            objArrayArray1[0x4b] = new object[] { "psnr", 2, false };
            objArrayArray1[0x4c] = new object[] { "ssim", 2, false };
            objArrayArray1[0x4d] = new object[] { "threads", 0, 0, 1, 0x80 };
            objArrayArray1[0x4e] = new object[] { "thread-input", 2, false };
            object[][] objArray = objArrayArray1;
            int length = objArray.Length;
            while (index < length)
            {
                x264ConfigNode node = new x264ConfigNode();
                node.Name = (string) objArray[index][0];
                node.Type = (NodeType) Enum.Parse(typeof(NodeType), objArray[index][1].ToString());
                if (node.Type == NodeType.Num)
                {
                    node.DefaultNum = Convert.ToDouble(objArray[index][2]);
                    node.Num = node.DefaultNum;
                    node.MinNum = Convert.ToDouble(objArray[index][3]);
                    node.MaxNum = Convert.ToDouble(objArray[index][4]);
                }
                else if (node.Type == NodeType.StrOptionIndex)
                {
                    node.StrOptionIndex = (int)(objArray[index][2]);
                    node.DefaultStrOptionIndex = node.StrOptionIndex;
                    node.StrOptions = objArray[index][3] as string[];
                }
                else if (node.Type == NodeType.Bool)
                {
                    node.DefaultBool = (bool)(objArray[index][2]);
                    node.Bool = node.DefaultBool;
                }
                else if (node.Type == NodeType.Str)
                {
                    node.Str = (string) objArray[index][2];
                    node.DefaultStr = node.Str;
                }
                this._optionDict.Add(node.Name, node);
                index++;
            }
            this._optionDict["crf"].InUse = true;
            this._optionDict["qp"].InUse = false;
            this._optionDict["bitrate"].InUse = false;
            int num3 = 0;
            string[] strArray = new string[] { "partitions", "p8x8", "p4x4", "b8x8", "i8x8", "i4x4", "deblock1", "deblock2", "psy-rd1", "psy-rd2" };
            int num4 = strArray.Length;
            while (num3 < num4)
            {
                this._optionDict[strArray[num3]].Special = true;
                num3++;
            }
            int num5 = 0;
            string[] strArray2 = new string[] { "pass", "stats", "slow-firstpass", "ratetol", "cplxblur", "qblur" };
            int num6 = strArray2.Length;
            while (num5 < num6)
            {
                this._optionDict[strArray2[num5]].Locked = true;
                num5++;
            }
            this._presets = new Dictionary<string, object>();
            this._presets.Add("no-8x8dct", false);
            this._presets.Add("aq-mode", 1);
            this._presets.Add("b-adapt", 1);
            this._presets.Add("bframes", 3);
            this._presets.Add("no-cabac", false);
            this._presets.Add("no-deblock", false);
            this._presets.Add("no-mbtree", false);
            this._presets.Add("direct", 1);
            this._presets.Add("no-fast-pskip", false);
            this._presets.Add("me", 1);
            this._presets.Add("no-mixed-refs", false);
            this._presets.Add("partitions", 3);
            this._presets.Add("p8x8", true);
            this._presets.Add("p4x4", false);
            this._presets.Add("b8x8", true);
            this._presets.Add("i8x8", true);
            this._presets.Add("i4x4", true);
            this._presets.Add("merange", 0x10);
            this._presets.Add("rc-lookahead", 40);
            this._presets.Add("ref", 3);
            this._presets.Add("no-scenecut", false);
            this._presets.Add("subme", 7);
            this._presets.Add("trellis", 1);
            this._presets.Add("no-weightb", false);
            this._presets.Add("weightp", 2);
        }

        private void Disable(IEnumerable<string> options)
        {
            foreach (string str in options)
            {
                this._optionDict[str].Locked = true;
            }
        }

        private void Disable(params string[] options)
        {
            int index = 0;
            string[] strArray = options;
            int length = strArray.Length;
            while (index < length)
            {
                this._optionDict[strArray[index]].Locked = true;
                index++;
            }
        }

        private void Enable(IEnumerable<string> options)
        {
            foreach (string str in options)
            {
                this._optionDict[str].Locked = false;
            }
        }

        private void Enable(params string[] options)
        {
            int index = 0;
            string[] strArray = options;
            int length = strArray.Length;
            while (index < length)
            {
                this._optionDict[strArray[index]].Locked = false;
                index++;
            }
        }

        private void Enable(bool restoreDefault, params string[] options)
        {
            if (restoreDefault)
            {
                int index = 0;
                string[] strArray = options;
                int length = strArray.Length;
                while (index < length)
                {
                    if (this._optionDict[strArray[index]].Locked)
                    {
                        this.RestoreDefault(strArray[index]);
                    }
                    index++;
                }
            }
            this.Enable((IEnumerable<string>) options);
        }

        public override string GetArgument()
        {
            if (this._usingCustomCmd && this._customCmdLine != null)
                return this._customCmdLine;
            string s = string.Empty;
            x264ConfigNode node = this._optionDict["pass"];
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            if (this._totalPass == 1)
            {
                node.Num = 0;
            }
            else if (this._totalPass == 2)
            {
                node.Num = this._currentPass;
            }
            else if (this._totalPass == 3)
            {
                node.Num = this._currentPass;
                if (node.Num == 3)
                {
                    node.Num = 2;
                }
                else if (node.Num == 2)
                {
                    node.Num = 3;
                }
            }
            foreach (x264ConfigNode node2 in this._optionDict.Values)
            {
                try
                {
                    if (!node2.Locked && node2.InUse)
                    {
                        if ((node2.Type == NodeType.Num) && (node2.Num != node2.DefaultNum))
                        {
                            if (!node2.Special)
                            {
                                s += new StringBuilder(" --").Append(node2.Name).Append(" ").Append(node2.Num).ToString();
                            }
                            else
                            {
                                double num;
                                double num2;
                                if (!flag && node2.Name.StartsWith("deblock"))
                                {
                                    num = this._optionDict["deblock1"].Num;
                                    num2 = this._optionDict["deblock2"].Num;
                                    s += new StringBuilder(" --deblock ").Append(num).Append(":").Append(num2).ToString();
                                    flag = true;
                                }
                                else if (!flag2 && node2.Name.StartsWith("psy-rd"))
                                {
                                    num = this._optionDict["psy-rd1"].Num;
                                    num2 = this._optionDict["psy-rd2"].Num;
                                    s += new StringBuilder(" --psy-rd ").Append(num).Append(":").Append(num2).ToString();
                                    flag2 = true;
                                }
                            }
                        }
                        else if ((node2.Type == NodeType.Bool) && (node2.Bool != node2.DefaultBool))
                        {
                            if (!node2.Special)
                            {
                                s += new StringBuilder(" --").Append(node2.Name).ToString();
                            }
                        }
                        else if ((node2.Type == NodeType.StrOptionIndex) && (node2.StrOptionIndex != node2.DefaultStrOptionIndex))
                        {
                            if (!node2.Special)
                            {
                                string[] strOptions = node2.StrOptions;
                                if (strOptions[node2.StrOptionIndex] != null)
                                {
                                    string[] array = node2.StrOptions;
                                    s += new StringBuilder(" --").Append(node2.Name).Append(" ").Append(array[node2.StrOptionIndex]).ToString();
                                    continue;
                                }
                            }
                            if (!flag3)
                            {
                                if (this._optionDict["partitions"].StrOptionIndex == 0)
                                {
                                    s += " --partitions all";
                                }
                                else if (this._optionDict["partitions"].StrOptionIndex == 1)
                                {
                                    s += " --partitions none";
                                }
                                else if (this._optionDict["partitions"].StrOptionIndex == 2)
                                {
                                    s += " --partitions ";
                                    int index = 0;
                                    string[] strArray = new string[] { "p8x8", "p4x4", "b8x8", "i8x8", "i4x4" };
                                    int length = strArray.Length;
                                    while (index < length)
                                    {
                                        x264ConfigNode node3 = this._optionDict[strArray[index]];
                                        if (!node3.Locked && node3.Bool)
                                        {
                                            s += new StringBuilder().Append(strArray[index]).Append(",").ToString();
                                        }
                                        index++;
                                    }
                                    if (s.EndsWith(","))
                                    {
                                        s = s.Substring(0, s.Length - 1);
                                    }
                                }
                                flag3 = true;
                            }
                        }
                        else if (node2.Str != node2.DefaultStr)
                        {
                            s += new StringBuilder(" --").Append(node2.Name).Append(" ").Append(node2.Str).ToString();
                        }
                    }
                    continue;
                }
                catch (Exception exception)
                {
                    MessageBox.Show((("发生了一个错误。\n" + node2.StrOptionIndex.ToString()) + "\n") + exception.ToString());
                    continue;
                }
            }
            return s;
        }

        public string[] GetDisabledOptions()
        {  
            List<string> options = new List<string>();
            foreach (x264ConfigNode node in this._optionDict.Values)
            {
                if (node.Locked)
                {
                    options.Add(node.Name);
                }

            }
            return options.ToArray();
        }

        public x264ConfigNode GetNode(string name)
        {
            try
            {
                return this._optionDict[name];
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void RefreshEnable(string lastSetOption)
        {
            try
            {
                string[] lhs = new string[] { "pass", "ratetol", "cplxblur", "qblur" };
                string[] rhs = new string[] { "stats", "slow-firstpass" };
                string[] strArray2 = new string[] { "pass", "ratetol", "cplxblur", "qblur", "stats", "slow-firstpass" };
                string[] strArray3 = new string[] { "qpmin", "qpmax", "qpstep", "ipratio", "pbratio", "chroma-qp-offset", "qcomp" };
                if (!this._optionDict["bitrate"].Locked)
                {
                    this.Enable((IEnumerable<string>) lhs);
                    this.Enable((IEnumerable<string>) strArray3);
                    string[] options = new string[] { "crf-max" };
                    this.Disable(options);
                }
                else if (!this._optionDict["crf"].Locked)
                {
                    string[] textArray5 = new string[] { "crf-max" };
                    this.Enable(textArray5);
                    this.Enable((IEnumerable<string>) strArray3);
                    this.Disable((IEnumerable<string>) strArray2);
                }
                else if (!this._optionDict["qp"].Locked)
                {
                    this.Disable((IEnumerable<string>) strArray3);
                    this.Disable((IEnumerable<string>) strArray2);
                    string[] textArray6 = new string[] { "crf-max" };
                    this.Disable(textArray6);
                }
                if (this._optionDict["no-scenecut"] != null)
                {
                    string[] textArray7 = new string[] { "scenecut" };
                    this.Disable(textArray7);
                }
                else
                {
                    string[] textArray8 = new string[] { "scenecut" };
                    this.Enable(textArray8);
                }
                if (this._optionDict["profile"].StrOptionIndex == 1)
                {
                    this._optionDict["no-8x8dct"].Bool = true;
                    this._optionDict["bframes"].Num = 0;
                    this._optionDict["no-cabac"].Bool = true;
                    this._optionDict["cqm"].StrOptionIndex = 1;
                    this._optionDict["weightp"].Num = 0;
                    this._optionDict["tff"].Bool = false;
                    this._optionDict["bff"].Bool = false;
                    string[] textArray9 = new string[] { "no-8x8dct", "bframes", "no-cabac", "cqm", "weightp", "tff", "bff" };
                    this.Disable(textArray9);
                }
                else if (this._optionDict["profile"].StrOptionIndex == 2)
                {
                    this._optionDict["no-8x8dct"].Bool = true;
                    this._optionDict["cqm"].StrOptionIndex = 1;
                    string[] textArray10 = new string[] { "no-8x8dct", "cqm" };
                    this.Disable(textArray10);
                }
                else
                {
                    string[] textArray11 = new string[] { "no-8x8dct", "bframes", "no-cabac", "cqm", "weightp", "tff", "bff" };
                    this.Enable(true, textArray11);
                }
                if (this._optionDict["no-cabac"].Bool)
                {
                    this._optionDict["trellis"].Num = 0;
                    string[] textArray12 = new string[] { "trellis" };
                    this.Disable(textArray12);
                }
                else
                {
                    string[] textArray13 = new string[] { "trellis" };
                    this.Enable(true, textArray13);
                }
                if ((this._optionDict["subme"].Num < 6) || this._optionDict["no-psy"].Bool)
                {
                    string[] textArray14 = new string[] { "psy-rd1" };
                    this.Disable(textArray14);
                }
                else
                {
                    string[] textArray15 = new string[] { "psy-rd1" };
                    this.Enable(textArray15);
                }
                if ((this._optionDict["trellis"].Num == 0) || this._optionDict["no-psy"].Bool)
                {
                    string[] textArray16 = new string[] { "psy-rd2" };
                    this.Disable(textArray16);
                }
                else
                {
                    string[] textArray17 = new string[] { "psy-rd2" };
                    this.Enable(textArray17);
                }
                if (this._optionDict["no-deblock"].Bool)
                {
                    string[] textArray18 = new string[] { "deblock1", "deblock2" };
                    this.Disable(textArray18);
                }
                else
                {
                    string[] textArray19 = new string[] { "deblock1", "deblock2" };
                    this.Enable(textArray19);
                }
                if (this._optionDict["aq-mode"].Num == 0)
                {
                    string[] textArray20 = new string[] { "aq-strength" };
                    this.Disable(textArray20);
                }
                else
                {
                    string[] textArray21 = new string[] { "aq-strength" };
                    this.Enable(textArray21);
                }
                bool flag = this._optionDict["partitions"].StrOptionIndex == 0 || this._optionDict["partitions"].StrOptionIndex == 1 || this._optionDict["partitions"].StrOptionIndex == 3;
                if (flag)
                {
                    string[] textArray22 = new string[] { "p8x8", "b8x8", "i4x4" };
                    this.Disable(textArray22);
                }
                else
                {
                    string[] textArray23 = new string[] { "p8x8", "b8x8", "i4x4" };
                    this.Enable(textArray23);
                }
                if (!this._optionDict["p8x8"].Bool || flag)
                {
                    string[] textArray24 = new string[] { "p4x4" };
                    this.Disable(textArray24);
                }
                else
                {
                    string[] textArray25 = new string[] { "p4x4" };
                    this.Enable(textArray25);
                }
                if (this._optionDict["no-8x8dct"].Bool || flag)
                {
                    string[] textArray26 = new string[] { "i8x8" };
                    this.Disable(textArray26);
                }
                else
                {
                    string[] textArray27 = new string[] { "i8x8" };
                    this.Enable(textArray27);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("发生了一个错误。\n" + exception.ToString());
            }
        }

        private void RestoreDefault(string name)
        {
            x264ConfigNode node = this._optionDict[name];
            if (node.Type == NodeType.Bool)
            {
                node.Bool = node.DefaultBool;
            }
            else if (node.Type == NodeType.Num)
            {
                node.Num = node.DefaultNum;
            }
            else if (node.Type == NodeType.StrOptionIndex)
            {
                node.StrOptionIndex = node.DefaultStrOptionIndex;
            }
            else if (node.Type == NodeType.Str)
            {
                node.Str = node.DefaultStr;
            }
        }

        public void SelectStringOption(string name, int index)
        {
            x264ConfigNode node = this._optionDict[name];
            if (!node.Locked)
            {
                node.StrOptionIndex = index;
                this.SettleConflicts(name, (double) index);
            }
        }

        public void SetBooleanOption(string name, bool value)
        {
            x264ConfigNode node = this._optionDict[name];
            if (!node.Locked)
            {
                node.Bool = value;
                this.RefreshEnable(name);
            }
        }

        public void SetNumOption(string name, double value)
        {
            x264ConfigNode node = this._optionDict[name];
            if (!node.Locked)
            {
                if (node.MinNum > value)
                {
                    value = node.MinNum;
                }
                if (node.MaxNum < value)
                {
                    value = node.MaxNum;
                }
                node.Num = value;
                this.SettleConflicts(name, value);
            }
        }

        private void SetOptionAndDefault(string name, object value)
        {
            x264ConfigNode node = this._optionDict[name];
            if (node.Type == NodeType.Bool)
            {
                node.Bool = Convert.ToBoolean(value);
                node.DefaultBool = node.Bool;
            }
            else if (node.Type == NodeType.Num)
            {
                node.Num = Convert.ToDouble(value);
                node.DefaultNum = node.Num;
            }
            else if (node.Type == NodeType.StrOptionIndex)
            {
                node.StrOptionIndex = Convert.ToInt32(value);
                node.DefaultStrOptionIndex = node.StrOptionIndex;
            }
            else if (node.Type == NodeType.Str)
            {
                node.Str = Convert.ToString(value);
                node.DefaultStr = node.Str;
            }
        }

        private void SetOptionsAndDefaults(Dictionary<string, object> namesAndValues)
        {
            IDictionaryEnumerator enumerator = namesAndValues.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, object> current = (KeyValuePair<string, object>) enumerator.Current;
                this.SetOptionAndDefault((string) current.Key, current.Value);
                this.RefreshEnable((string) current.Key);
            }
        }

        public void SetStringOption(string name, string value)
        {
            x264ConfigNode node = this._optionDict[name];
            if (!node.Locked)
            {
                node.Str = value;
            }
        }

        private void SettleConflicts(string lastSetName, double lastSetValue)
        {
            try
            {
                if (lastSetName == "preset")
                {
                    Dictionary<string, object> hash1 = new Dictionary<string, object>();
                    hash1.Add("no-8x8dct", false);
                    hash1.Add("aq-mode", 1);
                    hash1.Add("b-adapt", 1);
                    hash1.Add("bframes", 3);
                    hash1.Add("no-cabac", false);
                    hash1.Add("no-deblock", false);
                    hash1.Add("no-mbtree", false);
                    hash1.Add("direct", 1);
                    hash1.Add("no-fast-pskip", false);
                    hash1.Add("me", 1);
                    hash1.Add("no-mixed-refs", false);
                    hash1.Add("partitions", 3);
                    hash1.Add("p8x8", true);
                    hash1.Add("p4x4", false);
                    hash1.Add("b8x8", true);
                    hash1.Add("i8x8", true);
                    hash1.Add("i4x4", true);
                    hash1.Add("merange", 0x10);
                    hash1.Add("rc-lookahead", 40);
                    hash1.Add("ref", 3);
                    hash1.Add("no-scenecut", false);
                    hash1.Add("subme", 7);
                    hash1.Add("trellis", 1);
                    hash1.Add("no-weightb", false);
                    hash1.Add("weightp", 2);
                    this._presets = hash1;
                    if (lastSetValue == 0)
                    {
                        Dictionary<string, object> hash2 = new Dictionary<string, object>();
                        hash2.Add("no-8x8dct", true);
                        hash2.Add("aq-mode", 0);
                        hash2.Add("b-adapt", 0);
                        hash2.Add("bframes", 0);
                        hash2.Add("no-cabac", true);
                        hash2.Add("no-deblock", true);
                        hash2.Add("no-mbtree", true);
                        hash2.Add("me", 0);
                        hash2.Add("no-mixed-refs", true);
                        hash2.Add("partitions", 1);
                        hash2.Add("p8x8", false);
                        hash2.Add("b8x8", false);
                        hash2.Add("p4x4", false);
                        hash2.Add("i8x8", false);
                        hash2.Add("i4x4", false);
                        hash2.Add("ref", 1);
                        hash2.Add("no-scenecut", true);
                        hash2.Add("subme", 0);
                        hash2.Add("trellis", 0);
                        hash2.Add("no-weightb", true);
                        hash2.Add("weightp", 0);
                        IDictionaryEnumerator enumerator = hash2.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            KeyValuePair<string, object> current = (KeyValuePair<string, object>) enumerator.Current;
                            this._presets[current.Key] = current.Value;
                        }
                    }
                    else if (lastSetValue == 1)
                    {
                        Dictionary<string, object> hash3 = new Dictionary<string, object>();
                        hash3.Add("no-mbtree", true);
                        hash3.Add("me", 0);
                        hash3.Add("no-mixed-refs", true);
                        hash3.Add("partitions", 3);
                        hash3.Add("p8x8", false);
                        hash3.Add("b8x8", false);
                        hash3.Add("p4x4", false);
                        hash3.Add("i8x8", true);
                        hash3.Add("i4x4", true);
                        hash3.Add("ref", 1);
                        hash3.Add("subme", 1);
                        hash3.Add("trellis", 0);
                        hash3.Add("weightp", 0);
                        IDictionaryEnumerator enumerator2 = hash3.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            KeyValuePair<string, object> entry2 = (KeyValuePair<string, object>) enumerator2.Current;
                            this._presets[entry2.Key] = entry2.Value;
                        }
                    }
                    else if (lastSetValue == 2)
                    {
                        Dictionary<string, object> hash4 = new Dictionary<string, object>();
                        hash4.Add("no-mbtree", true);
                        hash4.Add("no-mixed-refs", true);
                        hash4.Add("ref", 1);
                        hash4.Add("subme", 2);
                        hash4.Add("trellis", 0);
                        hash4.Add("weightp", 0);
                        IDictionaryEnumerator enumerator3 = hash4.GetEnumerator();
                        while (enumerator3.MoveNext())
                        {
                            KeyValuePair<string, object> entry3 = (KeyValuePair<string, object>) enumerator3.Current;
                            this._presets[entry3.Key] = entry3.Value;
                        }
                    }
                    else if (lastSetValue == 3)
                    {
                        Dictionary<string, object> hash5 = new Dictionary<string, object>();
                        hash5.Add("no-mixed-refs", true);
                        hash5.Add("rc-lookahead", 20);
                        hash5.Add("ref", 2);
                        hash5.Add("subme", 4);
                        hash5.Add("weightp", 1);
                        IDictionaryEnumerator enumerator4 = hash5.GetEnumerator();
                        while (enumerator4.MoveNext())
                        {
                            KeyValuePair<string, object> entry4 = (KeyValuePair<string, object>) enumerator4.Current;
                            this._presets[entry4.Key] = entry4.Value;
                        }
                    }
                    else if (lastSetValue == 4)
                    {
                        Dictionary<string, object> hash6 = new Dictionary<string, object>();
                        hash6.Add("rc-lookahead", 30);
                        hash6.Add("ref", 2);
                        hash6.Add("subme", 6);
                        IDictionaryEnumerator enumerator5 = hash6.GetEnumerator();
                        while (enumerator5.MoveNext())
                        {
                            KeyValuePair<string, object> entry5 = (KeyValuePair<string, object>) enumerator5.Current;
                            this._presets[entry5.Key] = entry5.Value;
                        }
                    }
                    else if (lastSetValue != 5)
                    {
                        if (lastSetValue == 6)
                        {
                            Dictionary<string, object> hash7 = new Dictionary<string, object>();
                            hash7.Add("b-adapt", 2);
                            hash7.Add("direct", 3);
                            hash7.Add("me", 2);
                            hash7.Add("rc-lookahead", 50);
                            hash7.Add("ref", 5);
                            hash7.Add("subme", 8);
                            IDictionaryEnumerator enumerator6 = hash7.GetEnumerator();
                            while (enumerator6.MoveNext())
                            {
                                KeyValuePair<string, object> entry6 = (KeyValuePair<string, object>) enumerator6.Current;
                                this._presets[entry6.Key] = entry6.Value;
                            }
                        }
                        else if (lastSetValue == 7)
                        {
                            Dictionary<string, object> hash8 = new Dictionary<string, object>();
                            hash8.Add("b-adapt", 2);
                            hash8.Add("direct", 3);
                            hash8.Add("me", 2);
                            hash8.Add("partitions", 0);
                            hash8.Add("p8x8", true);
                            hash8.Add("p4x4", true);
                            hash8.Add("b8x8", true);
                            hash8.Add("i8x8", true);
                            hash8.Add("i4x4", true);
                            hash8.Add("rc-lookahead", 60);
                            hash8.Add("ref", 8);
                            hash8.Add("subme", 9);
                            hash8.Add("trellis", 2);
                            IDictionaryEnumerator enumerator7 = hash8.GetEnumerator();
                            while (enumerator7.MoveNext())
                            {
                                KeyValuePair<string, object> entry7 = (KeyValuePair<string, object>) enumerator7.Current;
                                this._presets[entry7.Key] = entry7.Value;
                            }
                        }
                        else if (lastSetValue == 8)
                        {
                            Dictionary<string, object> hash9 = new Dictionary<string, object>();
                            hash9.Add("b-adapt", 2);
                            hash9.Add("bframes", 8);
                            hash9.Add("direct", 3);
                            hash9.Add("me", 2);
                            hash9.Add("merange", 0x18);
                            hash9.Add("partitions", 0);
                            hash9.Add("p8x8", true);
                            hash9.Add("p4x4", true);
                            hash9.Add("b8x8", true);
                            hash9.Add("i8x8", true);
                            hash9.Add("i4x4", true);
                            hash9.Add("ref", 0x10);
                            hash9.Add("subme", 10);
                            hash9.Add("trellis", 2);
                            hash9.Add("rc-lookahead", 60);
                            IDictionaryEnumerator enumerator8 = hash9.GetEnumerator();
                            while (enumerator8.MoveNext())
                            {
                                KeyValuePair<string, object> entry8 = (KeyValuePair<string, object>) enumerator8.Current;
                                this._presets[entry8.Key] = entry8.Value;
                            }
                        }
                        else if (lastSetValue == 9)
                        {
                            Dictionary<string, object> hash10 = new Dictionary<string, object>();
                            hash10.Add("b-adapt", 2);
                            hash10.Add("bframes", 0x10);
                            hash10.Add("direct", 3);
                            hash10.Add("no-fast-pskip", true);
                            hash10.Add("me", 4);
                            hash10.Add("merange", 0x18);
                            hash10.Add("partitions", 0);
                            hash10.Add("p4x4", true);
                            hash10.Add("b8x8", true);
                            hash10.Add("i8x8", true);
                            hash10.Add("i4x4", true);
                            hash10.Add("ref", 0x10);
                            hash10.Add("subme", 10);
                            hash10.Add("trellis", 2);
                            hash10.Add("rc-lookahead", 60);
                            IDictionaryEnumerator enumerator9 = hash10.GetEnumerator();
                            while (enumerator9.MoveNext())
                            {
                                KeyValuePair<string, object> entry9 = (KeyValuePair<string, object>) enumerator9.Current;
                                this._presets[entry9.Key] = entry9.Value;
                            }
                        }
                    }
                    this.SetOptionsAndDefaults(this._presets);
                    if (this._optionDict["tune"].StrOptionIndex > 1)
                    {
                        this.SettleConflicts("tune", (double) this._optionDict["tune"].StrOptionIndex);
                    }
                }
                else if (lastSetName == "tune")
                {
                    Dictionary<string, object> namesAndValues = new Dictionary<string, object>();
                    namesAndValues.Add("bframes", this._presets["bframes"]);
                    namesAndValues.Add("no-cabac", this._presets["no-cabac"]);
                    namesAndValues.Add("ref", this._presets["ref"]);
                    namesAndValues.Add("no-deblock", this._presets["no-deblock"]);
                    namesAndValues.Add("deblock1", 0);
                    namesAndValues.Add("deblock2", 0);
                    namesAndValues.Add("rc-lookahead", this._presets["rc-lookahead"]);
                    namesAndValues.Add("ipratio", 1.4);
                    namesAndValues.Add("pbratio", 1.3);
                    namesAndValues.Add("aq-mode", this._presets["aq-mode"]);
                    namesAndValues.Add("aq-strength", 1.0);
                    namesAndValues.Add("qcomp", 0.6);
                    namesAndValues.Add("no-weightb", this._presets["no-weightb"]);
                    namesAndValues.Add("weightp", this._presets["weightp"]);
                    namesAndValues.Add("psy-rd1", 1);
                    namesAndValues.Add("psy-rd2", 0);
                    namesAndValues.Add("no-psy", false);
                    namesAndValues.Add("no-dct-decimate", false);
                    namesAndValues.Add("deadzone-inter", 0x15);
                    namesAndValues.Add("deadzone-intra", 11);
                    this.SetOptionsAndDefaults(namesAndValues);
                    if (lastSetValue != 0)
                    {
                        if (lastSetValue == 1)
                        {
                            Dictionary<string, object> hash12 = new Dictionary<string, object>();
                            hash12.Add("deblock1", -1);
                            hash12.Add("deblock2", -1);
                            hash12.Add("psy-rd2", 0.15);
                            this.SetOptionsAndDefaults(hash12);
                        }
                        else if (lastSetValue == 2)
                        {
                            int num = (int)(this._presets["bframes"]) + 2;
                            if (num > 0x10)
                            {
                                num = 0x10;
                            }
                            int num2 = (int)(this._presets["ref"]) * 2;
                            if (num2 > 0x10)
                            {
                                num2 = 0x10;
                            }
                            Dictionary<string, object> hash13 = new Dictionary<string, object>();
                            hash13.Add("bframes", num);
                            hash13.Add("ref", num2);
                            hash13.Add("deblock1", 1);
                            hash13.Add("deblock2", 1);
                            hash13.Add("aq-strength", 0.6);
                            hash13.Add("psy-rd1", 0.4);
                            this.SetOptionsAndDefaults(hash13);
                        }
                        else if (lastSetValue == 3)
                        {
                            Dictionary<string, object> hash14 = new Dictionary<string, object>();
                            hash14.Add("deblock1", -2);
                            hash14.Add("deblock2", -2);
                            hash14.Add("ipratio", 1.1);
                            hash14.Add("pbratio", 1.1);
                            hash14.Add("aq-strength", 0.5);
                            hash14.Add("qcomp", 0.8);
                            hash14.Add("psy-rd2", 0.25);
                            hash14.Add("no-dct-decimate", true);
                            hash14.Add("deadzone-inter", 6);
                            hash14.Add("deadzone-intra", 6);
                            this.SetOptionsAndDefaults(hash14);
                        }
                        else if (lastSetValue == 4)
                        {
                            Dictionary<string, object> hash15 = new Dictionary<string, object>();
                            hash15.Add("deblock1", -3);
                            hash15.Add("deblock2", -3);
                            hash15.Add("aq-strength", 1.2);
                            hash15.Add("psy-rd1", 2);
                            hash15.Add("psy-rd2", 0.7);
                            this.SetOptionsAndDefaults(hash15);
                        }
                        else if (lastSetValue == 5)
                        {
                            Dictionary<string, object> hash16 = new Dictionary<string, object>();
                            hash16.Add("aq-mode", 0);
                            hash16.Add("no-psy", true);
                            this.SetOptionsAndDefaults(hash16);
                        }
                        else if (lastSetValue == 6)
                        {
                            Dictionary<string, object> hash17 = new Dictionary<string, object>();
                            hash17.Add("aq-mode", 2);
                            hash17.Add("no-psy", true);
                            this.SetOptionsAndDefaults(hash17);
                        }
                        else if (lastSetValue == 7)
                        {
                            Dictionary<string, object> hash18 = new Dictionary<string, object>();
                            hash18.Add("no-cabac", true);
                            hash18.Add("no-deblock", true);
                            hash18.Add("no-weightb", true);
                            hash18.Add("weightp", 0);
                            this.SetOptionsAndDefaults(hash18);
                        }
                        else if (lastSetValue == 8)
                        {
                            Dictionary<string, object> hash19 = new Dictionary<string, object>();
                            hash19.Add("bframes", 0);
                            hash19.Add("rc-lookahead", 0);
                            this.SetOptionsAndDefaults(hash19);
                        }
                        else if (lastSetValue == 9)
                        {
                            this.SetOptionsAndDefaults(new Dictionary<string, object>());
                        }
                    }
                }
                else if (lastSetName == "qp")
                {
                    this._optionDict["qp"].InUse = true;
                    this._optionDict["crf"].InUse = false;
                    this._optionDict["bitrate"].InUse = false;
                }
                else if (lastSetName == "crf")
                {
                    this._optionDict["crf"].InUse = true;
                    this._optionDict["qp"].InUse = false;
                    this._optionDict["bitrate"].InUse = false;
                }
                else if (lastSetName == "bitrate")
                {
                    this._optionDict["bitrate"].InUse = true;
                    this._optionDict["crf"].InUse = false;
                    this._optionDict["qp"].InUse = false;
                }
                else if ((lastSetName == "subme") && (lastSetValue == 10))
                {
                    if ((this._optionDict["trellis"].Locked || this._optionDict["aq-mode"].Locked) || ((this._optionDict["trellis"].Num != 2) || (this._optionDict["aq-mode"].Num == 0)))
                    {
                        MessageBox.Show("subme 10 requires trellis = 2, aq-mode > 0 to take effect.");
                    }
                }
                else if (lastSetName == "partitions")
                {
                    if (lastSetValue == 0)
                    {
                        int index = 0;
                        string[] strArray = new string[] { "p8x8", "p4x4", "b8x8", "i8x8", "i4x4" };
                        int length = strArray.Length;
                        while (index < length)
                        {
                            if (!this._optionDict[strArray[index]].Locked)
                            {
                                this._optionDict[strArray[index]].Bool = true;
                            }
                            index++;
                        }
                    }
                    else if (lastSetValue == 1)
                    {
                        int num5 = 0;
                        string[] strArray2 = new string[] { "p8x8", "p4x4", "b8x8", "i8x8", "i4x4" };
                        int num6 = strArray2.Length;
                        while (num5 < num6)
                        {
                            if (!this._optionDict[strArray2[num5]].Locked)
                            {
                                this._optionDict[strArray2[num5]].Bool = false;
                            }
                            num5++;
                        }
                    }
                    else if (lastSetValue == 3)
                    {
                        int num7 = 0;
                        string[] strArray3 = new string[] { "p8x8", "p4x4", "b8x8", "i8x8", "i4x4" };
                        int num8 = strArray3.Length;
                        while (num7 < num8)
                        {
                            if (!this._optionDict[strArray3[num7]].Locked)
                            {
                                this._optionDict[strArray3[num7]].Bool = this._optionDict[strArray3[num7]].DefaultBool;
                            }
                            num7++;
                        }
                    }
                    this.RefreshEnable(lastSetName);
                }
                else if (lastSetName == "p8x8")
                {
                    if (!this._optionDict["p8x8"].Bool)
                    {
                        this._optionDict["p4x4"].Bool = false;
                    }
                    this.RefreshEnable(lastSetName);
                }
                else if (lastSetName == "no-8x8dct")
                {
                    if (this._optionDict["no-8x8dct"].Bool)
                    {
                        this._optionDict["i8x8"].Bool = false;
                    }
                    this.RefreshEnable(lastSetName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show((("发生了一个错误。\n" + lastSetName) + "\n") + exception.ToString());
            }
        }

        public int CurrentPass
        {
            get
            {
                return this._currentPass;
            }
            set
            {
                this._currentPass = value;
            }
        }

        public int TotalPass
        {
            get
            {
                return this._totalPass;
            }
            set
            {
                this._totalPass = value;
                if (value > 1)
                {
                    this._optionDict["pass"].Locked = false;
                    this._optionDict["stats"].Locked = false;
                    this._optionDict["slow-firstpass"].Locked = false;
                }
                else
                {
                    this._optionDict["pass"].Locked = true;
                    this._optionDict["stats"].Locked = true;
                    this._optionDict["slow-firstpass"].Locked = true;
                }
            }
        }
    }
}

