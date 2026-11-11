using System;
using System.Text;

namespace Evergreen.TutorDemo
{
    public enum DemoReplyKind
    {
        None,
        Answer,
        Decline,
        TopicFallback,
        GenericFallback,
    }

    public readonly struct DemoReply
    {
        public readonly DemoReplyKind Kind;
        public readonly string Text;

        public DemoReply(DemoReplyKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }
    }

    // Picks a scripted reply: best keyword score in the active topic, then off-topic decline,
    // then the topic fallback, then the generic fallback.
    public static class DemoTutorMatcher
    {
        public static DemoReply Match(DemoTutorScript script, string behaviourKey, string question)
        {
            string q = Normalize(question);
            if (script == null || q.Length == 0) return new DemoReply(DemoReplyKind.None, null);

            DemoTutorTopic topic = FindTopic(script, behaviourKey);
            if (topic != null)
            {
                DemoTutorEntry best = Best(topic.entries, q);
                if (best != null) return new DemoReply(DemoReplyKind.Answer, best.answer);
            }

            DemoTutorEntry decline = Best(script.declineEntries, q);
            if (decline != null) return new DemoReply(DemoReplyKind.Decline, decline.answer);

            if (topic != null && !string.IsNullOrWhiteSpace(topic.fallback))
                return new DemoReply(DemoReplyKind.TopicFallback, topic.fallback);

            return new DemoReply(DemoReplyKind.GenericFallback, script.genericFallback);
        }

        static DemoTutorTopic FindTopic(DemoTutorScript script, string key)
        {
            if (script.topics == null || string.IsNullOrWhiteSpace(key)) return null;
            foreach (var t in script.topics)
                if (t != null && string.Equals(t.behaviourKey, key.Trim(), StringComparison.OrdinalIgnoreCase))
                    return t;
            return null;
        }

        // Highest score wins; on a tie the earlier entry wins.
        static DemoTutorEntry Best(DemoTutorEntry[] entries, string normalizedQuestion)
        {
            if (entries == null) return null;
            DemoTutorEntry best = null;
            int bestScore = 0;
            string padded = " " + normalizedQuestion + " ";
            foreach (var e in entries)
            {
                if (e == null || e.keywords == null) continue;
                int score = 0;
                foreach (var k in e.keywords)
                {
                    string nk = Normalize(k);
                    if (nk.Length > 0 && padded.Contains(nk)) score++;
                }
                if (score > bestScore)
                {
                    best = e;
                    bestScore = score;
                }
            }
            return best;
        }

        // Lowercase, letters and digits only, single spaces.
        public static string Normalize(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return "";
            var sb = new StringBuilder(s.Length);
            bool space = false;
            foreach (char c in s.Trim().ToLowerInvariant())
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                    space = false;
                }
                else if (!space && sb.Length > 0)
                {
                    sb.Append(' ');
                    space = true;
                }
            }
            return sb.ToString().TrimEnd();
        }
    }
}
