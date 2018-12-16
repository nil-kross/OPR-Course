using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Ans.ComponentSystem;
using Ans.ComponentSystem.Interop;
using Ansys.DesignXplorer.API.Common;
using Ansys.DesignXplorer.API.Optimization;
using Ans.DesignXplorer.InterProcessConnectionServer;
using Ans.DesignXplorer.InterProcessConnectionServices;
using Ans.DesignXplorer.CommonMatlabApplication;

namespace Ans.DesignXplorer.MatlabOptimizer {
    public abstract class baseOptimizer : IOptimizationMethod, ICostFunctionEvaluator {
        protected readonly List<Parameter> _parameters = new List<Parameter>();
        protected readonly List<Constraint> _constraints = new List<Constraint>();
        private readonly List<Objective> _objectives = new List<Objective>();
        private IOptimizationServices _services;

        private readonly IListCpp _samples = new VariantColl();
        private readonly IListCpp _candidates = new VariantColl();

        static baseOptimizer() {
            MiniCom.AnsSetRegistryLoadConfiguration(MiniCom.ANSRegistryLoadConfig.ANSYS_RegistryLoadConfig_AnsRegistry);
            MiniCom.AnsCoInitialize(IntPtr.Zero);
        }

        #region IOptimizationMethod Members

        public void AddBooleanVariable(string bsUniqueID, bool initialvalue) {
            throw new NotImplementedException();
        }

        public void AddDoubleListVariable(string bsUniqueID, Ans.ComponentSystem.IListCpp values, double initialvalue) {
            throw new NotImplementedException();
        }

        public void AddDoubleVariable(string bsUniqueID, double lowerbound, double upperbound, double initialvalue) {
            _parameters.Add(new Parameter { ID = bsUniqueID, LowerBound = lowerbound, UpperBound = upperbound, InitialValue = initialvalue });
        }

        public void AddIntegerListVariable(string bsUniqueID, Ans.ComponentSystem.IListCpp values, int initialvalue) {
            throw new NotImplementedException();
        }

        public void AddIntegerVariable(string bsUniqueID, int lowerbound, int upperbound, int initialvalue) {
            throw new NotImplementedException();
        }

        public void AddCustomVariableProperty(string bsVariableID, string bsPropertyKey, object vntPropertyValue) {
            throw new NotImplementedException();
        }

        public void AddObjective(string bsUniqueObjectiveID, enumGoalType type, double objectivevalue, double importance) {
            if (type == enumGoalType.eGT_NoPreference)
                return;
            _objectives.Add(new Objective { ID = bsUniqueObjectiveID, ObjectiveType = type, ObjectiveValue = objectivevalue, Importance = importance });
        }

        public void AddConstraint(string bsVariableID, enumConstraintType type, object vntConstraintValue1, object vntConstraintValue2, double dblImportance, bool vbStrictConstraint) {
            if (type == enumConstraintType.eCT_NoPreference)
                return;
            _constraints.Add(new Constraint { ConstraintType = type, ID = bsVariableID, Value1 = (double)vntConstraintValue1, Value2 = (double)vntConstraintValue2, Importance = dblImportance, IsStrict = vbStrictConstraint });
        }

        public void AddCustomObjectiveProperty(string bsVariableID, string bsPropertyKey, object vntPropertyValue) {
            throw new NotImplementedException();
        }

        public void AddCustomConstraintProperty(string bsVariableID, string bsPropertyKey, object vntPropertyValue) {
            throw new NotImplementedException();
        }

        public void AddOutput(string bsUniqueOutputID) {
            throw new NotImplementedException();
        }

        public void AddParameterRelationship(string bsVariableID, string bsLeftExpression, string bsRightExpression, enumParameterRelationshipType eParameterRelationshipType) {
            throw new NotImplementedException();
        }

        public bool CanRun(out string bsMessage) {
            bsMessage = "Go";
            return true;
        }

        public Ans.ComponentSystem.IListCpp Candidates {
            get { return _candidates; }
        }

        public enumPostProcessingType PostProcessingTypes {
            get { return enumPostProcessingType.ePPT_Candidates | enumPostProcessingType.ePPT_Samples; }
        }

        public virtual LogLevel GetLogLevel() {
            return LogLevel.All;
        }

        public abstract bool PushOptions(MatlabApplication matlab);

        public abstract void RunOptimizer(MatlabApplication matlab, string startingpoint, string lowerbounds, string upperbounds);

        public abstract void GetResults(MatlabApplication matlab);

        public void Run() {
            using (IServer server = new Server<DXServices>()) {
                DXServices services = server.GetServices() as DXServices;
                services.SetEvaluator(this);
                using (dynamic matlab = new MatlabApplication(false, GetLogLevel(), cmd => _services.PublishLogMessage(cmd))) {
                    matlab.AddReferenceToAssembly("Ans.DesignXplorer.InterProcessConnectionClient.dll");

                    matlab.Execute("import InterProcessConnectionClient.*");

                    matlab.AddReferenceToAssembly("Ans.DesignXplorer.InterProcessConnectionServices.dll");

                    matlab.Execute("import InterProcessConnectionServices.*");

                    string command = string.Format("optimconvergence = NET.createGeneric('Ans.DesignXplorer.InterProcessConnectionClient.RemoteClient',{1}'Ans.DesignXplorer.InterProcessConnectionServices.DXServices'{2},{0})", server.GetPort(), '{', '}');
                    matlab.Execute(command);

                    matlab.AddMLibraryPath();
                    bool lic = PushOptions(matlab);
                    if (!lic)
                        return;

                    // Definition of starting point values [value_1;value_2;...;value_N]
                    StringBuilder sb_X0 = new StringBuilder("[");
                    foreach (Parameter parameter in _parameters) {
                        sb_X0.Append(parameter.InitialValue.ToString("R", CultureInfo.InvariantCulture));
                        sb_X0.Append(';');
                    }
                    sb_X0.Remove(sb_X0.Length - 1, 1);
                    sb_X0.Append(']');

                    // Definition of lower bound values [value_1;value_2;...;value_N]
                    StringBuilder sb_lb = new StringBuilder("[");
                    foreach (Parameter parameter in _parameters) {
                        sb_lb.Append(parameter.LowerBound.ToString("R", CultureInfo.InvariantCulture));
                        sb_lb.Append(';');
                    }
                    sb_lb.Remove(sb_lb.Length - 1, 1);
                    sb_lb.Append(']');

                    // Definition of upper bound values [value_1;value_2;...;value_N]
                    StringBuilder sb_ub = new StringBuilder("[");
                    foreach (Parameter parameter in _parameters) {
                        sb_ub.Append(parameter.UpperBound.ToString("R", CultureInfo.InvariantCulture));
                        sb_ub.Append(';');
                    }
                    sb_ub.Remove(sb_ub.Length - 1, 1);
                    sb_ub.Append(']');

                    // Anonymous function to convert a double array from C# (index 0 based) in a double array for matlab (index 1 based)
                    command = string.Format("convertToDoubleArray = @(x,myfun)(double(myfun(x)));");
                    matlab.Execute(command);

                    // Anonymous function to define the cost function
                    command = string.Format("fun = @(x)convertToDoubleArray(x,@(x)optimconvergence.VectorFunction(x));");
                    matlab.Execute(command);

                    string nonlcon = string.Format("nonlcon = [];");
                    if (_constraints.Count > 0) {
                        // anonymous function to define function with 2 output arguments, where the 1st output correspond to inequalities, and the 2nd to equalities
                        command = string.Format("constraintfunction = @(x,inequalities)deal(double(inequalities(x)),([]));");
                        matlab.Execute(command);

                        nonlcon = string.Format("nonlcon = @(x)constraintfunction(x,@(x)optimconvergence.InequalitiesFunction(x));");
                    }
                    matlab.Execute(nonlcon);

                    RunOptimizer(matlab, sb_X0.ToString(), sb_lb.ToString(), sb_ub.ToString());

                    try {
                        if (_parameters.Count > 1) {
                            double[,] values = matlab.GetVariable("x");
                            OutputResult(values);
                            IEnumerator enume = values.GetEnumerator();

                            foreach (IOptimizationPoint dxOptimizationPoint in _samples) {
                                enume.Reset();
                                bool same = true;
                                foreach (Parameter parameter in _parameters) {
                                    enume.MoveNext();
                                    double cVal = (double)enume.Current;
                                    double pVal = Convert.ToDouble(dxOptimizationPoint.get_Value(parameter.ID));
                                    if (cVal != 0.0) {
                                        if (Math.Abs((cVal - pVal) / cVal) > 1e-10) {
                                            same = false;
                                            break;
                                        }
                                    } else if (pVal != 0.0) {
                                        same = false;
                                        break;
                                    }
                                }
                                if (same) {
                                    _candidates.Add(dxOptimizationPoint);
                                    break;
                                }
                            }
                        } else {
                            double values = matlab.GetVariable("x");
                            OutputResult(values);
                            foreach (IOptimizationPoint dxOptimizationPoint in _samples) {
                                bool same = true;
                                Parameter parameter = _parameters[0];
                                double cVal = values;
                                double pVal = Convert.ToDouble(dxOptimizationPoint.get_Value(parameter.ID));
                                if (cVal != 0.0) {
                                    if (Math.Abs((cVal - pVal) / cVal) > 1e-10)
                                        same = false;
                                } else if (pVal != 0.0) {
                                    same = false;
                                }

                                if (same) {
                                    _candidates.Add(dxOptimizationPoint);
                                    break;
                                }
                            }
                        }

                        GetResults(matlab);
                    } catch (Exception) {
                        //throw;
                    }
                }

                services.Summary();
            }
        }

        public Ans.ComponentSystem.IListCpp Samples {
            get { return _samples; }
        }

        public IOptimizationServices Services {
            set { _services = value; }
        }

        public abstract object get_Setting(string bsSetting);

        public abstract void put_Setting(string bsSetting, object pvntVal);

        #endregion

        protected static void OutputResult(object result) {
            Array arr = result as System.Array;

            if (arr != null) {
                foreach (object o in arr)
                    Console.WriteLine("arr result = {0}", o);
            } else
                Console.WriteLine("result = {0}", result);

        }

        private IOptimizationPoint ComputePoint(double[] x) {
            try {
                IOptimizationPoint pt = null;
                for (int ipt = _samples.Count - 1; ipt >= 0; ipt--) {
                    IOptimizationPoint dxOptimizationPoint = _samples[ipt] as IOptimizationPoint;
                    bool same = true;
                    int ind = 0;
                    foreach (Parameter parameter in _parameters) {
                        double cVal = x[ind];
                        double pVal = Convert.ToDouble(dxOptimizationPoint.get_Value(parameter.ID));
                        if (cVal != 0.0) {
                            if (Math.Abs((cVal - pVal) / cVal) > 1e-10) {
                                same = false;
                                break;
                            }
                        } else if (pVal != 0.0) {
                            same = false;
                            break;
                        }
                        ind++;
                    }
                    if (same) {
                        pt = dxOptimizationPoint;
                        break;
                    }
                }
                if (pt == null) {
                    pt = new DXOptimizationPoint();
                    int ind = 0;
                    foreach (Parameter parameter in _parameters) {
                        double val = x[ind];
                        //if (val < parameter.LowerBound || val > parameter.UpperBound)
                        //{
                        //    cost = double.MaxValue;
                        //    return _services.Stopped; 
                        //}
                        pt.put_Value(parameter.ID, val);
                        ind++;
                    }
                    _services.CalculatePoint(pt);

                    if (pt.State == enumPointState.ePS_UpToDate) {
                        _samples.Add(pt);
                    }
                }

                return pt;
            } catch (Exception e) {
                return null;
            }
        }

        #region ICostFunctionEvaluator Members

        public bool Evaluate(double[] x, out double cost) {
            try {
                IOptimizationPoint pt = ComputePoint(x);
                if (pt != null && pt.State == enumPointState.ePS_UpToDate) {
                    _services.PushHistoryPoint(pt);

                    cost = 0.0;
                    foreach (Objective o in _objectives)
                        cost = cost + o.Compute(pt);
                    //foreach (Constraint c in _constraints)
                    //    cost = cost + c.Compute(pt);
                } else {
                    cost = double.MaxValue;
                }
            } catch (Exception e) {
                cost = double.MaxValue;
                Console.WriteLine(e);
            }
            return _services.Stopped;
        }

        public bool EvaluateVector(double[] x, out double[] values) {
            values = new double[_objectives.Count];
            try {
                IOptimizationPoint pt = ComputePoint(x);
                if (pt != null && pt.State == enumPointState.ePS_UpToDate) {
                    _services.PushHistoryPoint(pt);
                    int io = 0;
                    foreach (Objective o in _objectives) {
                        values[io] = o.Compute(pt);
                        io++;
                    }
                    //foreach (Constraint c in _constraints)
                    //    cost = cost + c.Compute(pt);
                } else {
                    int io = 0;
                    foreach (Objective o in _objectives) {
                        values[io] = double.MaxValue;
                        io++;
                    }
                }
            } catch (Exception e) {
                int io = 0;
                foreach (Objective o in _objectives) {
                    values[io] = double.MaxValue;
                    io++;
                }
                Console.WriteLine(e);
            }
            return _services.Stopped;
        }

        public void EvaluateInequalities(double[] x, out double[] inequalities) {
            inequalities = null;

            if (_constraints.Count == 0)
                return;

            inequalities = new double[_constraints.Count];
            try {
                IOptimizationPoint pt = ComputePoint(x);
                if (pt != null && pt.State == enumPointState.ePS_UpToDate) {
                    int ic = 0;
                    foreach (Constraint c in _constraints) {
                        inequalities[ic] = c.Compute(pt);
                        ic++;
                    }
                } else {
                    int ic = 0;
                    foreach (Constraint c in _constraints) {
                        inequalities[ic] = double.MaxValue;
                        ic++;
                    }
                }
            } catch (Exception e) {
                int ic = 0;
                foreach (Constraint c in _constraints) {
                    inequalities[ic] = double.MaxValue;
                    ic++;
                }
                Console.WriteLine(e);
            }
            return;
        }

        public void EvaluateEqualities(double[] x, out double[] equalities) {
            equalities = null;
        }

        public void PushConvergenceData(int iteration, double metric) {
            IConvergenceData data = new DXConvergenceData();
            data.put_Value(0, iteration, metric);
            _services.PushConvergenceData(data);
        }

        #endregion
    }
}
