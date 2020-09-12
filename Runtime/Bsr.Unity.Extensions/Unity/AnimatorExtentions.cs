using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable UnusedMember.Global

namespace Bsr.Unity.Extensions.Unity
{
    /// <summary>
    /// Indicates where an animation callback will be fired on the animation clip timeline.
    /// Choose <see cref="ClipEventFirePosition.Custom"/> to set custom fire time.
    /// </summary>
    public enum ClipEventFirePosition
    {
        Custom = 0,
        AtStart,
        AtEnd,
        AtMiddle
    }

    // ReSharper disable once UnusedType.Global
    public static class AnimatorExtentions
    {
        public static void SetOverride(this Animator animator, AnimatorOverrideController overrideController)
        {
            animator.runtimeAnimatorController = overrideController;
        }

        public static AnimatorOverrideController CreateOverride(
            this Animator animator,
            string name,
            ICollection<AnimationClip> clips,
            IDictionary<string, string> map,
            Func<ICollection<AnimationClip>, ICollection<AnimationClip>, IDictionary<string, string>, IList<KeyValuePair<AnimationClip, AnimationClip>>> mapper)
        {
            var controller = animator.runtimeAnimatorController;
            var originClips = controller.animationClips;

            var overrideController = new AnimatorOverrideController(controller) {name = name};
            overrideController.ApplyOverrides(mapper(originClips, clips, map));

            return overrideController;
        }

        public static IList<KeyValuePair<AnimationClip, AnimationClip>> MapByOrder(this ICollection<AnimationClip> origins, ICollection<AnimationClip> clips)
        {
            var mapped = new List<KeyValuePair<AnimationClip, AnimationClip>>(origins.Count);

            using (var clipsEnumerator = clips.GetEnumerator())
            {
                foreach (var origin in origins)
                {
                    var mapClip = clipsEnumerator.Current;
                    mapped.Add(new KeyValuePair<AnimationClip, AnimationClip>(origin, mapClip));

                    if (!clipsEnumerator.MoveNext())
                        break;
                }

                return mapped;
            }
        }

        public static IList<KeyValuePair<AnimationClip, AnimationClip>> MapByNames(this ICollection<AnimationClip> origins, ICollection<AnimationClip> clips, IDictionary<string, string> map)
        {
            var clipsDictionary = clips.ToDictionary();

            var mapped = new List<KeyValuePair<AnimationClip, AnimationClip>>(origins.Count);

            foreach (var origin in origins)
            {
                if (!map.TryGetValue(origin.name, out var mapName))
                    continue;

                if (!clipsDictionary.TryGetValue(mapName, out var mapClip))
                    continue;

                mapped.Add(new KeyValuePair<AnimationClip, AnimationClip>(origin, mapClip));
            }

            return mapped;
        }

        private static IDictionary<string, AnimationClip> ToDictionary(this ICollection<AnimationClip> clips)
        {
            var dictionary = new Dictionary<string, AnimationClip>(clips.Count);
            foreach (var clip in clips)
            {
                dictionary.Add(clip.name, clip);
            }

            return dictionary;
        }

        /// <summary>
        /// Divide clip length by 4 and add 2 animation events: at the beginning and near the end.
        /// </summary>
        /// <note>Example: If clip is length of 40 then events will be added to 10 nd 30 frames.</note>
        /// <param name="controller">Animator controller.</param>
        /// <param name="clipName">Animation clip name to attach the <paramref name="callbackMethodName"/> callback.</param>
        /// <param name="callbackMethodName">Method must be on <see cref="GameObject"/> with <see cref="Animator"/> component attached.</param>
        public static void BindRuntimeAnimationFootStepEventCallback(this RuntimeAnimatorController controller, string clipName, string callbackMethodName)
        {
            var clip = controller.GetAnimationClip(clipName);

            var forthPart = clip.length / 4f;

            var firstStep = new AnimationEvent
            {
                functionName = callbackMethodName,
                time = forthPart
            };

            var secondStep = new AnimationEvent
            {
                functionName = callbackMethodName,
                time = forthPart + forthPart + forthPart
            };

            clip.AddEvent(firstStep);
            clip.AddEvent(secondStep);
        }

        /// <summary>
        /// Add an event to the specified clip with the method callback.
        /// </summary>
        /// <param name="controller">Animator controller.</param>
        /// <param name="clipName">Animation clip name to attach the <paramref name="callbackMethodName"/> callback.</param>
        /// <param name="callbackMethodName">Method must be on <see cref="GameObject"/> with <see cref="Animator"/> component attached.</param>
        /// <param name="firePosition">Sets where an animation callback will be fired on the animation clip timeline.
        /// Choose <see cref="ClipEventFirePosition.Custom"/> to set custom fire time otherwise <paramref name="fireTime"/> will be ignored.</param>
        /// <param name="fireTime">Event fire time. Will be set to the clip max length if exceeded.</param>
        /// <exception cref="ArgumentOutOfRangeException">when <see cref="ClipEventFirePosition"/> value is out of enum range.</exception>
        public static void BindRuntimeAnimationEventCallback(this RuntimeAnimatorController controller, string clipName, string callbackMethodName, ClipEventFirePosition firePosition, float fireTime = 0f)
        {
            var clip = controller.GetAnimationClip(clipName);

            switch (firePosition)
            {
                case ClipEventFirePosition.Custom:
                {
                    if (fireTime > clip.length)
                        fireTime = clip.length;
                    break;
                }
                case ClipEventFirePosition.AtStart:
                {
                    fireTime = 0f;
                    break;
                }
                case ClipEventFirePosition.AtEnd:
                {
                    fireTime = clip.length;
                    break;
                }
                case ClipEventFirePosition.AtMiddle:
                {
                    fireTime = clip.length / 2;
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(firePosition), firePosition, null);
            }

            var animationEvent = new AnimationEvent
            {
                functionName = callbackMethodName,
                time = fireTime
            };

            clip.AddEvent(animationEvent);
        }

        /// <exception cref="KeyNotFoundException">when animation clip with provided name not found.</exception>
        public static AnimationClip GetAnimationClip(this RuntimeAnimatorController controller, string name)
        {
            var clips = controller.animationClips;

            for (var i = 0; i < clips.Length; i++)
            {
                if (clips[i].name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    return clips[i];
            }

            throw new KeyNotFoundException($"No animation clip with name {name} found.");
        }
    }
}