using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace AuthService.Extensions
{
    public static class StringExtensions
    {
        // used in searchers
        public static string AddDay(this string s)
        {
            if (!DateTime.TryParse(s, out var d))
            {
                return null;
            }

            var result = d.AddDays(1).ToString("d");

            return result;
        }

        public static string Center(this string s, char c, int w)
        {
            var p = Math.Max(0, (w - s.Length) / 2);

            if (p == 0)
            {
                return s;
            }

            if (p * 2 + s.Length != w)
            {
                s += " ";
            }

            var pl = $"{new string(c, p - 1)} ";
            var pr = $" {new string(c, p - 1)}";
            var result = $"{pl}{s}{pr}";

            return result;
        }

        public static string Clean(this string s)
        {
            var result =
              string.IsNullOrWhiteSpace(s)
                ? null
                : s;

            return result;
        }

        public static bool IsMultiline(this string s)
        {
            return s != null && s.Trim().Contains(Environment.NewLine);
        }

        public static double SimilarityPercentage(this string source, string target)
        {
            source = source?.ToLower();
            target = target?.ToLower();

            if (source == null || target == null)
            {
                return 0.0;
            }

            if (source.Length == 0 || target.Length == 0)
            {
                return 0.0;
            }

            if (source == target)
            {
                return 1.0;
            }

            var stepsToSame = ComputeLevenshteinDistance(source, target);
            var result = 1.0 - (double)stepsToSame / Math.Max(source.Length, target.Length);

            return result;
        }

        public static string SqlEncode(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            var result = s.Replace("'", "''").Replace("\"", "\"\"");

            return result;
        }

        public static DateTime ToDate(this string s)
        {
            var exception = new Exception("The date is invalid");

            if (string.IsNullOrWhiteSpace(s))
            {
                throw exception;
            }

            var valid = DateTime.TryParseExact(s, "M/d/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out var result);

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "MMddyyyy", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "yyyyMMdd", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "yyyyMMddHH", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "yyyyMMddHHzzz", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "yyyyMMddHHmm", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "yyyyMMddHHmmzzz", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "yyyyMMddHHmmss", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "yyyyMMddHHmmsszzz", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "M/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "yyyyMM", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParseExact(s, "M/d/yyyy H:mm", new CultureInfo("en-US"), DateTimeStyles.None, out result);
            }

            if (!valid)
            {
                valid = DateTime.TryParse(s, out result);
            }

            if (!valid)
            {
                throw exception;
            }

            var date = DateTime.UtcNow.AddYears(-150);

            if (result < date)
            {
                throw exception;
            }

            return result;
        }

        public static string ToDiagnosisCode(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            var result =
              s.Length > 3
                ? $"{s.Substring(0, 3)}.{s.Substring(3)}"
                : s;

            return result;
        }

        public static long ToInt32(this string s)
        {
            var result = int.Parse(s);

            return result;
        }

        public static long ToInt64(this string s)
        {
            var result = long.Parse(s);

            return result;
        }

        public static string ToLetterString(this string s)
        {
            var result =
              s == null
                ? null
                : Regex.Replace(s, "[^A-Za-z]", string.Empty, RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

            return result;
        }

        public static DateTime? ToNullableDate(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            var result = s.ToDate();

            return result;
        }

        public static DateTime? ToNullableDateFromChangeHealthcare(this string s)
        {
            var d = s.ToNullableDate();

            if (d == null)
            {
                return null;
            }

            var zone = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time");

            return TimeZoneInfo.ConvertTimeToUtc(d.Value, zone);
        }

        public static DateTime? ToNullableDateFromRcopia(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            var date = s[..^3];
            var d = date.ToDate();

            var id = s[date.Length..];
            var zone = GetTimeZone(id);

            return TimeZoneInfo.ConvertTimeToUtc(d, zone);
        }

        public static int? ToNullableInt32(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            int? result = null;

            if (int.TryParse(s, out var n))
            {
                result = n;
            }

            return result;
        }

        public static long? ToNullableInt64(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            long? result = null;

            if (long.TryParse(s, out var n))
            {
                result = n;
            }

            return result;
        }

        public static string ToNumberString(this string s, params string[] prefixes)
        {
            var result =
              s == null
                ? null
                : Regex.Replace(s, "[^0-9]", string.Empty, RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

            if (result == null)
            {
                return null;
            }

            if (prefixes == null)
            {
                return result;
            }

            foreach (var prefix in prefixes)
            {
                if (s.StartsWith(prefix))
                {
                    return prefix + result;
                }
            }

            return result;
        }

        public static string ToPhone(this string s, PhoneFormat format = 0)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            switch (format)
            {
                case PhoneFormat.Cda:
                    {
                        if (s.StartsWith("+"))
                        {
                            s = s.ToNumberString();
                        }

                        return
                          s.Length switch
                          {
                              11 => $"+{s[..1]}({s.Substring(1, 3)}){s.Substring(4, 3)}-{s.Substring(7, 4)}",
                              10 => $"+1({s[..3]}){s.Substring(3, 3)}-{s.Substring(6, 4)}",
                              7 => $"{s[..3]}-{s.Substring(3, 4)}",
                              _ => s
                          };
                    }
                case PhoneFormat.Standard:
                default:
                    {
                        if (s.StartsWith("+"))
                        {
                            return s;
                        }

                        return
                          s.Length switch
                          {
                              11 => $"{s[..1]} ({s.Substring(1, 3)}) {s.Substring(4, 3)}-{s.Substring(7, 4)}",
                              10 => $"({s[..3]}) {s.Substring(3, 3)}-{s.Substring(6, 4)}",
                              7 => $"{s[..3]}-{s.Substring(3, 4)}",
                              _ => s
                          };
                    }
            }
        }

        public static string[] ToPlainLines(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return Array.Empty<string>();
            }

            return Regex.Split(s, "\r\n|\r|\n").Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }

        public static string ToPostalCode(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return null;
            }

            if (s.Length == 5)
            {
                return s;
            }

            var result = $"{s.Substring(0, 5)}-{s.Substring(5, 4)}";

            return result;
        }

        public static string ToReadable(this string s)
        {
            var result =
              s == null
                ? null
                : Regex.Replace(s, "([a-z]|[A-Z]{2,})([A-Z])", "$1 $2");

            return result;
        }

        public static string ToSentenceCase(this string s)
        {
            if (s == null)
            {
                return null;
            }

            var builder = new StringBuilder(s.Length + Regex.Matches(s, "[A-Z]").Count);
            var buffer = new Stack<char>();
            var start = true;

            for (var i = 0; i < s.Length; i++)
            {
                var c =
                  i == 0
                    ? char.ToUpper(s[i])
                    : s[i];

                if (char.IsUpper(c))
                {
                    buffer.Push(c);
                }
                else
                {
                    if (buffer.Count != 0)
                    {
                        var l = char.ToLower(buffer.Pop());

                        var acronym =
                          buffer.Count == 0
                            ? null
                            : string.Join(null, buffer.Reverse());

                        if (start)
                        {
                            start = false;
                        }
                        else
                        {
                            builder.Append(" ");
                        }

                        if (acronym == null)
                        {
                            builder.Append(builder.Length == 0 ? char.ToUpper(l) : l);
                        }
                        else
                        {
                            builder
                              .Append(acronym)
                              .Append(" ")
                              .Append(l);
                        }

                        buffer.Clear();
                    }

                    builder.Append(c);
                }
            }

            if (buffer.Count == 0)
            {
                return builder.ToString();
            }

            if (!start)
            {
                builder.Append(" ");
            }

            builder.Append(string.Join(null, buffer.Reverse()));

            return builder.ToString();
        }

        public static string ToSsn(this string s)
        {
            var result =
              s == null
                ? null
                : $"{s.Substring(0, 3)}-{s.Substring(3, 2)}-{s.Substring(5, 4)}";

            return result;
        }

        public static string ToTitleCase(this string s)
        {
            var result =
              s == null
                ? null
                : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());

            return result;
        }

        public static string ToWysiwyg(this string s)
        {
            var result =
              string.IsNullOrWhiteSpace(s)
                ? string.Empty
                : s;

            return result;
        }

        private static int ComputeLevenshteinDistance(string source, string target)
        {
            if (source == null || target == null)
            {
                return 0;
            }

            if (source.Length == 0 || target.Length == 0)
            {
                return 0;
            }

            if (source == target)
            {
                return source.Length;
            }

            var sourceWordCount = source.Length;
            var targetWordCount = target.Length;

            // step 1
            if (sourceWordCount == 0)
            {
                return targetWordCount;
            }

            if (targetWordCount == 0)
            {
                return sourceWordCount;
            }

            var distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // step 2
            for (var i = 0; i <= sourceWordCount; distance[i, 0] = i++) { }

            for (var j = 0; j <= targetWordCount; distance[0, j] = j++) { }

            for (var i = 1; i <= sourceWordCount; i++)
            {
                for (var j = 1; j <= targetWordCount; j++)
                {
                    // step 3
                    var cost = target[j - 1] == source[i - 1] ? 0 : 1;

                    // step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            var result = distance[sourceWordCount, targetWordCount];

            return result;
        }

        private static TimeZoneInfo GetTimeZone(string id)
        {
            return id switch
            {
                "AST" => TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time"),
                "ADT" => TimeZoneInfo.FindSystemTimeZoneById("Atlantic Standard Time"),
                "EST" => TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"),
                "EDT" => TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"),
                "CST" => TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"),
                "CDT" => TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"),
                "MST" => TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"),
                "MDT" => TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"),
                "PST" => TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
                "PDT" => TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
                "AKST" => TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time"),
                "AKDT" => TimeZoneInfo.FindSystemTimeZoneById("Alaskan Standard Time"),
                "HST" => TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
                "HDT" => TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
                "HAST" => TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
                "HADT" => TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time"),
                "SST" => TimeZoneInfo.FindSystemTimeZoneById("Samoa Standard Time"),
                "SDT" => TimeZoneInfo.FindSystemTimeZoneById("Samoa Standard Time"),
                _ => throw new InvalidOperationException($"ID is unsupported: {id}")
            };
        }
    }

    public enum PhoneFormat
    {
        Standard = 0,
        Cda = 1
    }
}