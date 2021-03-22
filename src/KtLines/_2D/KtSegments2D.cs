using EMDD.KtGeometry.KtPoints;

using KtExtensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EMDD.KtGeometry.KtLines._2D
{
    public static class KtSegments2DHelper
    {
    }

    public class KtSegments2D : IEnumerable<KtSegment2D>, IEquatable<KtSegments2D>
    {
        private readonly List<KtPoint2D> joints;

        public KtSegments2D(params KtPoint2D[] _joints)
        {
            joints = _joints.ToList();
        }

        public KtSegments2D(IEnumerable<KtPoint2D> _joints)
        {
            joints = _joints.ToList();
        }

        public IEnumerable<KtPoint2D> Joints => joints;

        public KtPoint2D Start => joints[0];

        public KtPoint2D End => joints.Last();

        public IEnumerator<KtSegment2D> GetEnumerator()
        {
            if (joints.Count < 2) yield break;
            var start = joints[0];
            foreach (var point in joints.Skip(1))
            {
                yield return new KtSegment2D(start, point);
                start = point;
            }
        }

        public bool Connectible(KtSegments2D segments, bool atStart) => atStart ? Start == segments.Start || Start == segments.End : End == segments.Start || End == segments.End;

        public KtSegments2D Connect(KtSegments2D segments, bool atStart)
        {
            if (atStart)
            {
                if (Start == segments.Start)
                {
                    return new KtSegments2D(JointStartToStart(segments));
                }
                else if (Start == segments.End)
                {
                    return new KtSegments2D(JointStartToEnd(segments));
                }
            }
            else if (End == segments.Start)
            {
                return new KtSegments2D(JointEndToStart(segments));
            }
            else if (End == segments.End)
            {
                return new KtSegments2D(JointEndToEnd(segments));
            }
            return this;
        }

        //private bool TryConect(KtSegments2D segments, out IEnumerable<KtPoint2D> result)
        //{
        //    if (Start == segments.Start)
        //    {
        //        result=JointStartToStart(segments);
        //        return true;
        //    }
        //    else if (Start == segments.End)
        //    {
        //        result = JointStartToEnd(segments);
        //        return true;
        //    }
        //    else if (End == segments.Start)
        //    {
        //        result = JointEndToStart(segments);
        //        return true;
        //    }
        //    else if (End == segments.End)
        //    {
        //        result = JointEndToEnd(segments);
        //        return true;
        //    }
        //    result = null;
        //    return false;
        //}

        private IEnumerable<KtPoint2D> JointStartToStart(KtSegments2D segments) =>
            Joints.Reverse().Concat(segments.Joints.Skip(1));

        private IEnumerable<KtPoint2D> JointStartToEnd(KtSegments2D segments) =>
            segments.Joints.Concat(Joints.Skip(1));

        private IEnumerable<KtPoint2D> JointEndToStart(KtSegments2D segments) =>
            Joints.Concat(segments.Joints.Skip(1));

        private IEnumerable<KtPoint2D> JointEndToEnd(KtSegments2D segments) =>
            Joints.Concat(segments.Joints.Reverse().Skip(1));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsRegion() => Start == End;

        public (KtPolygons.Regions.KtRegion, KtSegments2D) Weld(KtSegments2D segments)
        {
            var tempListofJoints =
                Start == segments.Start ? JointStartToStart(segments) :
                Start == segments.End ? JointStartToEnd(segments) :
                End == segments.Start ? JointEndToStart(segments) :
                End == segments.End ? JointEndToEnd(segments) :
                null;
            if (tempListofJoints != null)
            {
                if (tempListofJoints.FirstOrDefault() == tempListofJoints.Last())
                    return (tempListofJoints.Skip(1).ToList(), null);
                return (null, new KtSegments2D(tempListofJoints));
            }
            return (null, null);
        }

        public IEnumerable<KtSegments2D> CutBy(KtSegment2D segment)
        {
            var listOfCuts = new List<KtSegments2D>();
            var list1 = new List<KtPoint2D> { this.FirstOrDefault()?.Start };
            using (var i = GetEnumerator())
            {
                while (i.MoveNext())
                {
                    var seg = i.Current;
                    var cut = seg.IntersectionWith(segment);
                    if (cut != null)
                    {
                        if (cut == seg.Start)
                        {
                            if (list1.Count > 1)
                            {
                                listOfCuts.Add(new KtSegments2D(list1));
                                list1 = new List<KtPoint2D> { cut };
                            }
                            list1.Add(seg.End);
                        }
                        else if (cut == seg.End)
                        {
                            list1.Add(cut);
                            listOfCuts.Add(new KtSegments2D(list1));
                            list1 = new List<KtPoint2D> { cut };
                        }
                        else
                        {
                            list1.Add(cut);
                            listOfCuts.Add(new KtSegments2D(list1));
                            list1 = new List<KtPoint2D> { cut };
                            list1.Add(seg.End);
                        }
                    }
                    else
                    {
                        list1.Add(seg.End);
                    }
                }
            }
            if (list1.Count > 1) listOfCuts.Add(new KtSegments2D(list1));
            return listOfCuts;
        }

        public bool Equals(KtSegments2D other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (this is null || other is null) return false;
            if (joints.Count != other.joints.Count) return false;
            var trueForward = true;
            var trueBackward = true;
            for (int i = 0; i < joints.Count; i++)
            {
                var joint1 = joints[i];
                var joint2 = other.joints[i];
                var joint3 = other.joints[joints.Count - i - 1];
                trueForward &= joint1 == joint2;
                trueBackward &= joint1 == joint3;
                if (!trueForward && !trueBackward) return false;
            }
            return trueForward || trueBackward;
        }

        public static bool operator ==(KtSegments2D a, KtSegments2D b) => a.DefaultEquals(b);

        public static bool operator !=(KtSegments2D a, KtSegments2D b) => !(a == b);

        public override bool Equals(object obj) => Equals(obj as KtSegments2D);

        public override int GetHashCode() => unchecked(joints.Aggregate(0, (total, joint) => total + (joint.GetHashCode() * 31)));
    }
}