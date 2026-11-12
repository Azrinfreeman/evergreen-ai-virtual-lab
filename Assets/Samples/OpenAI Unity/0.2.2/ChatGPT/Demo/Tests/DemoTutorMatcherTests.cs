using NUnit.Framework;
using UnityEngine;

namespace Evergreen.TutorDemo.Tests
{
    public class DemoTutorMatcherTests
    {
        DemoTutorScript script;

        [SetUp]
        public void SetUp()
        {
            script = ScriptableObject.CreateInstance<DemoTutorScript>();
            script.genericFallback = "generic";
            script.declineEntries = new[]
            {
                new DemoTutorEntry { keywords = new[] { "piala dunia" }, answer = "decline" },
            };
            script.topics = new[]
            {
                new DemoTutorTopic
                {
                    behaviourKey = "MM4",
                    fallback = "mm4 fallback",
                    entries = new[]
                    {
                        new DemoTutorEntry { keywords = new[] { "gfp", "pendarfluor" }, answer = "gfp answer" },
                        new DemoTutorEntry { keywords = new[] { "enzim sekatan", "xbai", "gfp" }, answer = "enzyme answer" },
                    },
                },
                new DemoTutorTopic { behaviourKey = "MM3", fallback = "", entries = new DemoTutorEntry[0] },
            };
        }

        [TearDown]
        public void TearDown() => Object.DestroyImmediate(script);

        [Test]
        public void ExactQuestion_ReturnsAnswer()
        {
            var r = DemoTutorMatcher.Match(script, "MM4", "Apakah GFP?");
            Assert.AreEqual(DemoReplyKind.Answer, r.Kind);
            Assert.AreEqual("gfp answer", r.Text);
        }

        [Test]
        public void RewordedQuestion_IgnoresCasePunctuationAndSpacing()
        {
            var r = DemoTutorMatcher.Match(script, "mm4", "  kenapa   guna ENZIM,  sekatan??  ");
            Assert.AreEqual("enzyme answer", r.Text);
        }

        [Test]
        public void HigherScoreWins()
        {
            var r = DemoTutorMatcher.Match(script, "MM4", "gfp dan enzim sekatan");
            Assert.AreEqual("enzyme answer", r.Text);
        }

        [Test]
        public void Tie_GoesToEarlierEntry()
        {
            var r = DemoTutorMatcher.Match(script, "MM4", "gfp");
            Assert.AreEqual("gfp answer", r.Text);
        }

        [Test]
        public void NoMatch_ReturnsTopicFallback()
        {
            var r = DemoTutorMatcher.Match(script, "MM4", "kenapa langit biru");
            Assert.AreEqual(DemoReplyKind.TopicFallback, r.Kind);
            Assert.AreEqual("mm4 fallback", r.Text);
        }

        [Test]
        public void OffTopic_ReturnsDecline()
        {
            var r = DemoTutorMatcher.Match(script, "MM4", "Siapa menang piala dunia");
            Assert.AreEqual(DemoReplyKind.Decline, r.Kind);
        }

        [Test]
        public void EmptyOrWhitespace_ReturnsNone()
        {
            Assert.AreEqual(DemoReplyKind.None, DemoTutorMatcher.Match(script, "MM4", "").Kind);
            Assert.AreEqual(DemoReplyKind.None, DemoTutorMatcher.Match(script, "MM4", "   ").Kind);
            Assert.AreEqual(DemoReplyKind.None, DemoTutorMatcher.Match(script, "MM4", null).Kind);
        }

        [Test]
        public void UnknownBehaviourKey_ReturnsGenericFallback()
        {
            var r = DemoTutorMatcher.Match(script, "Nope", "apakah gfp");
            Assert.AreEqual(DemoReplyKind.GenericFallback, r.Kind);
            Assert.AreEqual("generic", r.Text);
        }

        [Test]
        public void TopicWithEmptyFallback_UsesGenericFallback()
        {
            var r = DemoTutorMatcher.Match(script, "MM3", "soalan lain");
            Assert.AreEqual(DemoReplyKind.GenericFallback, r.Kind);
        }

        [Test]
        public void NullScript_ReturnsNone()
        {
            Assert.AreEqual(DemoReplyKind.None, DemoTutorMatcher.Match(null, "MM4", "gfp").Kind);
        }
    }
}
