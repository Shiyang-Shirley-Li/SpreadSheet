// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        private int graphSize;

        private Dictionary<String, HashSet<String>> dependents;//dependents have dependents as key and dependees as value

        private Dictionary<String, HashSet<String>> dependees;//dependees have dependees as key and dependents as value

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            graphSize = 0;
            dependents = new Dictionary<string, HashSet<String>>();
            dependees = new Dictionary<string, HashSet<String>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return graphSize; } 
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s] // s is a dependent
        {
            get
            {
                if (!dependents.ContainsKey(s))
                {
                    return 0;
                }
                return dependents[s].Count;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)// s is dependee
        {
            if (!dependees.ContainsKey(s))
            {
                return false;
            }
            else if(dependees[s].Count > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)// s is dependent
        {
            if (!dependents.ContainsKey(s))
            {
                return false; 
            }
            else if (dependents[s].Count > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)//s is dependee
        {
            if (dependees.ContainsKey(s))
            {
                return dependees[s];
            }
            return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)//s is dependent
        {
            if (dependents.ContainsKey(s))
            {
                return dependents[s];
            }
            return new HashSet<string>();
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (!dependees.ContainsKey(s))
            {
                HashSet<String> dependentsSet = new HashSet<string>();
                dependentsSet.Add(t);
                dependees.Add(s, dependentsSet);
                graphSize++;
            }
            else
            {
                dependees[s].Add(t);
                graphSize++;
            }
            if (!dependents.ContainsKey(t))
            {
                HashSet<String> dependeesSet = new HashSet<string>();
                dependeesSet.Add(s);
                dependents.Add(t, dependeesSet);
            }
            else
            {
                dependents[t].Add(s);
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if(this.HasDependees(t) && dependents[t].Contains(s))// || this.HasDependents(s) && dependees[s].Contains(t))//need two conditions?
            {
                dependents[t].Remove(s);
                dependees[s].Remove(t);
                graphSize--;
            }

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            
            HashSet<string> newDependentsSet = new HashSet<string>();
            foreach (string dependent in newDependents)
            {
                newDependentsSet.Add(dependent);
            }


            if (dependees.ContainsKey(s))
            {
                foreach (string dependent in dependees[s])
                {
                    dependents[dependent].Remove(s);
                }

                dependees[s].Clear();
                dependees[s] = newDependentsSet;

                foreach(string dependent in newDependentsSet)
                {
                    if (dependents.ContainsKey(dependent))
                    {
                        dependents[dependent].Add(s);
                    }
                    else
                    {
                        dependents.Add(dependent, new HashSet<string> { s });
                    }
                }
            }
            else
            {
                dependees.Add(s, newDependentsSet);
                foreach (string dependent in newDependentsSet)
                {
                    if (dependents.ContainsKey(dependent))
                    {
                        dependents[dependent].Add(s);
                    }
                    else
                    {
                        dependents.Add(dependent, new HashSet<string> { s });
                    }
                }
            }

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees) 
        {

            HashSet<string> newDependeesSet = new HashSet<string>();
            foreach (string dependee in newDependees)
            {
                newDependeesSet.Add(dependee);
            }


            if (dependents.ContainsKey(s))
            {
                foreach (string dependee in dependents[s])
                {
                    dependees[dependee].Remove(s);
                }

                dependents[s].Clear();
                dependents[s] = newDependeesSet;

                foreach (string dependee in newDependeesSet)
                {
                    if (dependees.ContainsKey(dependee))
                    {
                        dependees[dependee].Add(s);
                    }
                    else
                    {
                        dependees.Add(dependee, new HashSet<string> { s });
                    }
                }
            }
            else
            {
                dependents.Add(s, newDependeesSet);
                foreach (string dependee in newDependeesSet)
                {
                    if (dependees.ContainsKey(dependee))
                    {
                        dependents[dependee].Add(s);
                    }
                    else
                    {
                        dependents.Add(dependee, new HashSet<string> { s });
                    }
                }
            }

        }

    }

}
