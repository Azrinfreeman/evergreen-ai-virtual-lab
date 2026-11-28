using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Evergreen.TutorDemo;
#if UNITY_WEBGL && !UNITY_EDITOR
using UnityEngine;
#endif

namespace OpenAI
{
    // Supplies scripted tutor replies in the same response type the live call returns,
    // so ChatGPT.SendReplyAsync handles both the same way.
    public static class DemoTutorProvider
    {
        // Only checks that a key probably exists. It never opens, logs or shows the key.
        public static bool KeyAvailable()
        {
            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("OPENAI_API_KEY"))) return true;
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return File.Exists(Path.Combine(home, ".openai", "auth.json"));
        }

        public static async Task<CreateChatCompletionResponse> ReplyAsync(
            DemoTutorScript script, string behaviourKey, string question)
        {
            if (script.thinkingSeconds > 0f)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                // Wait on Unity's player loop, which runs in single-threaded browser builds.
                double replyTime = Time.realtimeSinceStartupAsDouble + script.thinkingSeconds;
                while (Time.realtimeSinceStartupAsDouble < replyTime)
                {
                    await Awaitable.NextFrameAsync();
                }
#else
                await Task.Delay((int)(script.thinkingSeconds * 1000f));
#endif
            }

            DemoReply reply = DemoTutorMatcher.Match(script, behaviourKey, question);
            return new CreateChatCompletionResponse
            {
                Choices = new List<ChatChoice>
                {
                    new ChatChoice
                    {
                        Index = 0,
                        Message = new ChatMessage { Role = "assistant", Content = reply.Text },
                    },
                },
            };
        }
    }
}
