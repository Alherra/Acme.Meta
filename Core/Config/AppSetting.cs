using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Meta
{
    /// <summary>
    /// Yaml配置
    /// </summary>
    [Description("Yaml配置")]
    public class AppSetting
    {
        /// <summary>
        /// Yaml功能
        /// </summary>
        [Description("Yaml功能")]
        static readonly YamlProcedure _yml = YamlProcedure.Instance(Path.Combine(Directory.GetCurrentDirectory(), "application.yml"));

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        [Description("获取")]
        public static string Get(string section)
        {
            return _yml.Read(section);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        [Description("编辑")]
        public static void Set(string section, string val)
        {
            _yml.Modify(section, val);
            _yml.Save();
        }
    }

    /// <summary>
    /// YAML功能
    /// </summary>
    [Description("YAML功能")]
    class YamlProcedure
    {
        /// <summary>
        /// 所有行
        /// </summary>
        [Description("所有行")]
        private readonly String[] _lines;

        /// <summary>
        /// 格式化为节点
        /// </summary>
        [Description("格式化为节点")]
        private List<Node> _nodeList = new();

        /// <summary>
        /// 文件所在地址
        /// </summary>
        [Description("文件所在地址")]
        private readonly String _path;

        /// <summary>
        /// 私有化实例
        /// </summary>
        [Description("私有化实例")]
        private YamlProcedure(String path)
        {
            this._path = path;
            this._lines = File.ReadAllLines(path);

            for (int i = 0; i < _lines.Length; i++)
            {
                String line = _lines[i];
                if (line.Trim() == "")
                {
                    //Console.WriteLine("空白行，行号：" + (i + 1));
                    continue;
                }
                else if (line.Trim()[..1] == "#")
                {
                    //Console.WriteLine("注释行，行号：" + (i + 1));
                    continue;
                }

                String[] kv = Regex.Split(line, ":", RegexOptions.IgnoreCase);
                FindPreSpace(line);
                Node node = new();
                node.Space = FindPreSpace(line);
                node.Name = kv[0].Trim();

                // 去除前后空白符
                String fline = line.Trim();
                int first = fline.IndexOf(':');
                node.Value = first == fline.Length - 1 ? null! : fline.Substring(first + 2, fline.Length - first - 2);
                node.Parent = FindParent(node.Space);
                _nodeList.Add(node);
            }

            this.Formatting();
        }

        /// <summary>
        /// 生成实例
        /// </summary>
        [Description("生成实例")]
        public static YamlProcedure Instance(String path)
        {
            return new YamlProcedure(path);
        }

        /// <summary>
        /// 编辑
        /// 
        /// 允许key为多级
        /// 例如：spring.datasource.url
        /// </summary>
        [Description("编辑")]
        public void Modify(String key, String value)
        {
            Node node = FindNodeByKey(key);
            if (node != null)
            {
                node.Value = value;
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        [Description("读取")]
        public String Read(String key)
        {
            Node node = FindNodeByKey(key);
            if (node != null)
            {
                return node.Value;
            }
            return null!;
        }

        /// <summary>
        /// 根据key找节点
        /// </summary>
        [Description("根据key找节点")]
        private Node FindNodeByKey(String key)
        {
            String[] ks = key.Split('.');
            for (int i = 0; i < _nodeList.Count; i++)
            {
                Node node = _nodeList[i];
                if (node.Name == ks[^1])
                {
                    // 判断父级
                    Node tem = node;
                    // 统计匹配到的次数
                    int count = 1;
                    for (int j = ks.Length - 2; j >= 0; j--)
                    {
                        if (tem.Parent.Name == ks[j])
                        {
                            count++;
                            // 继续检查父级
                            tem = tem.Parent;
                        }
                    }

                    if (count == ks.Length)
                    {
                        return node;
                    }
                }
            }
            return null!;
        }

        /// <summary>
        /// 保存到文件中
        /// </summary>
        [Description("保存到文件中")]
        public void Save()
        {
            StreamWriter stream = File.CreateText(this._path);
            for (int i = 0; i < _nodeList.Count; i++)
            {
                Node node = _nodeList[i];
                StringBuilder sb = new();
                // 放入前置空格
                for (int j = 0; j < node.Tier; j++)
                {
                    sb.Append("  ");
                }
                sb.Append(node.Name);
                sb.Append(": ");
                if (node.Value != null)
                {
                    sb.Append(node.Value);
                }
                stream.WriteLine(sb.ToString());
            }
            stream.Flush();
            stream.Close();
        }

        /// <summary>
        /// 格式化
        /// </summary>
        [Description("格式化")]
        public void Formatting()
        {
            // 先找出根节点
            List<Node> parentNode = new();
            for (int i = 0; i < _nodeList.Count; i++)
            {
                Node node = _nodeList[i];
                if (node.Parent == null)
                {
                    parentNode.Add(node);
                }
            }

            List<Node> fNodeList = new();
            // 遍历根节点
            for (int i = 0; i < parentNode.Count; i++)
            {
                Node node = parentNode[i];
                fNodeList.Add(node);
                FindChildren(node, fNodeList);
            }

            //Console.WriteLine("完成");

            // 指针指向格式化后的
            _nodeList = fNodeList;
        }

        /// <summary>
        /// 层级
        /// </summary>
        [Description("层级")]
        int tier = 0;

        /// <summary>
        /// 查找子集并进行分层
        /// </summary>
        [Description("查找子集并进行分层")]
        private void FindChildren(Node node, List<Node> fNodeList)
        {
            // 当前层 默认第一级，根在外层进行操作
            tier++;

            for (int i = 0; i < _nodeList.Count; i++)
            {
                Node item = _nodeList[i];
                if (item.Parent == node)
                {
                    item.Tier = tier;
                    fNodeList.Add(item);
                    FindChildren(item, fNodeList);
                }
            }

            // 走出一层
            tier--;
        }

        /// <summary>
        /// 查找前缀空格数量
        /// </summary>
        [Description("查找前缀空格数量")]
        private static int FindPreSpace(String str)
        {
            List<char> chars = str.ToList();
            int count = 0;
            foreach (char c in chars)
            {
                if (c == ' ')
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        /// <summary>
        /// 根据缩进找上级
        /// </summary>
        [Description("根据缩进找上级")]
        private Node FindParent(int space)
        {

            if (_nodeList.Count == 0)
            {
                return null!;
            }
            else
            {
                // 倒着找上级
                for (int i = _nodeList.Count - 1; i >= 0; i--)
                {
                    Node node = _nodeList[i];
                    if (node.Space < space)
                    {
                        return node;
                    }
                }
                // 如果没有找到 返回null
                return null!;
            }
        }

        /// <summary>
        /// 私有节点类
        /// </summary>
        [Description("私有节点类")]
        private class Node
        {
            // 名称
            public string Name { get; set; } = String.Empty;
            // 值
            public string Value { get; set; } = String.Empty;
            // 父级
            public Node Parent { get; set; } = null!;
            // 前缀空格
            public int Space { get; set; }
            // 所属层级
            public int Tier { get; set; }
        }
    }
}
