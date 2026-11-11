using System;
using UnityEngine;

namespace Evergreen.TutorDemo
{
    public enum TutorMode
    {
        [Tooltip("Use the live OpenAI tutor when a key exists, otherwise the scripted replies.")]
        Auto,
        Live,
        Demo,
    }

    [Serializable]
    public class DemoTutorEntry
    {
        [Tooltip("Any one of these words or phrases in the question selects this answer. More matches score higher.")]
        public string[] keywords;

        [TextArea(2, 6)]
        public string answer;

        [Tooltip("Where in the project the facts come from. Never shown to the player.")]
        public string source;
    }

    [Serializable]
    public class DemoTutorTopic
    {
        [Tooltip("Same key as CheckBehaviour.BehaviourName, for example MM4. Not case sensitive.")]
        public string behaviourKey;

        public DemoTutorEntry[] entries;

        [TextArea(2, 6)]
        [Tooltip("Reply when no entry matches. Lists what the player can ask.")]
        public string fallback;
    }

    // Fixed replies for the tutor chat when no OpenAI key is available.
    // Edit the content in the Inspector; the matching rules live in DemoTutorMatcher.
    [CreateAssetMenu(fileName = "DemoTutorScript", menuName = "Evergreen/Demo Tutor Script")]
    public class DemoTutorScript : ScriptableObject
    {
        public DemoTutorTopic[] topics;

        [TextArea(2, 6)]
        [Tooltip("Reply for a topic that has no entry in the list above.")]
        public string genericFallback;

        [Tooltip("Off-topic questions. The answer field is the decline reply.")]
        public DemoTutorEntry[] declineEntries;

        [Tooltip("Pause before the reply so the 'Sedang menyediakan jawapan...' state is visible.")]
        [Min(0f)]
        public float thinkingSeconds = 0.8f;

        [Tooltip("Status line shown after each scripted reply.")]
        public string notice = "Mod demo: jawapan contoh, bukan daripada model AI.";
    }
}
