using Ans.ComponentSystem;
using Ans.DesignXplorer.InterProcessConnectionServer;
using Ans.DesignXplorer.InterProcessConnectionServices;
using Ansys.ACT.Interfaces.Common;
using Ansys.DesignXplorer.API.Common;
using Ansys.DesignXplorer.API.Optimization;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ansys {
    public class GenAlOptimizer : IOptimizationMethod, ICostFunctionEvaluator {
        private readonly Dictionary<String, Object> settings = new Dictionary<String, Object>() {
            ["Generations"] = 10
        };
        private readonly List<Parameter> parameters = new List<Parameter>();
        private readonly List<Constraint> constraints = new List<Constraint>();
        private readonly List<Objective> objectives = new List<Objective>();
        private readonly IListCpp samples = new VariantColl();
        private readonly IListCpp candidates = new VariantColl();
        private IOptimizationServices services;
        private readonly IExtAPI api;

        public IOptimizationServices Services {
            get { return this.services; }
            set { this.services = value; }
        }

        public IListCpp Candidates {
            get { return this.candidates; }
        }

        public IListCpp Samples {
            get { return this.samples; }
        }

        public enumPostProcessingType PostProcessingTypes {
            get { return enumPostProcessingType.ePPT_Candidates | enumPostProcessingType.ePPT_Samples; }
        }

        public GenAlOptimizer(IExtAPI extensionApi) {
            this.api = extensionApi;
        }

        public Object get_Setting(String bsSetting) {
            return this.settings[bsSetting];
        }

        public void put_Setting(String bsSetting, Object vntVal) {
            this.settings[bsSetting] = vntVal;
        }

        public void AddDoubleVariable(String bsUniqueID, Double lowerbound, Double upperbound, Double initialvalue) {
            this.parameters.Add(new Parameter {
                ID = bsUniqueID,
                LowerBound = lowerbound,
                UpperBound = upperbound,
                InitialValue = initialvalue
            });
        }

        public void AddIntegerListVariable(String bsVariableID, Ans.ComponentSystem.IListCpp pValues, Int32 iInitialValue) {
            throw new NotImplementedException();
        }

        public void AddDoubleListVariable(String bsVariableID, Ans.ComponentSystem.IListCpp pValues, Double dblInitialValue) {
            throw new NotImplementedException();
        }

        public void AddOutput(String bsVariableID) {
            throw new NotImplementedException();
        }

        public void AddParameterRelationship(String bsVariableID, String bsLeftExpression, String bsRightExpression, enumParameterRelationshipType eParameterRelationshipType) {
            throw new NotImplementedException();
        }

        public void AddCustomVariableProperty(String bsVariableID, String bsPropertyKey, Object vntPropertyValue) {
            throw new NotImplementedException();
        }

        public void AddObjective(String bsUniqueObjectiveID, enumGoalType type, Double objectivevalue, Double importance) {
            if (type == enumGoalType.eGT_NoPreference)
                return;
            this.objectives.Add(new Objective {
                ID = bsUniqueObjectiveID,
                ObjectiveType = type,
                ObjectiveValue = objectivevalue,
                Importance = importance
            });
        }

        public void AddConstraint(
            String bsVariableID,
            enumConstraintType type,
            Object vntConstraintValue1,
            Object vntConstraintValue2,
            Double dblImportance,
            Boolean vbStrictConstraint
        ) {
            if (type == enumConstraintType.eCT_NoPreference)
                return;
            this.constraints.Add(new Constraint {
                ConstraintType = type,
                ID = bsVariableID,
                Value1 = (double)vntConstraintValue1,
                Value2 = (double)vntConstraintValue2,
                Importance = dblImportance,
                IsStrict = vbStrictConstraint
            });
        }

        public void AddCustomObjectiveProperty(String bsVariableID, String bsPropertyKey, Object vntPropertyValue) {
            throw new NotImplementedException();
        }

        public void AddCustomConstraintProperty(String bsVariableID, String bsPropertyKey, Object vntPropertyValue) {
            throw new NotImplementedException();
        }

        public Boolean CanRun(out String bsErrorMessage) {
            bsErrorMessage = "Go";

            return true;
        }

        public void EvaluateEqualities(Double[] x, out Double[] equalities) {
            equalities = null;
        }

        private IOptimizationPoint ComputePoint(Double[] x) {
            try {
                IOptimizationPoint pt = null;

                for (int ipt = this.samples.Count - 1; ipt >= 0; ipt--) {
                    IOptimizationPoint dxOptimizationPoint = this.samples[ipt] as IOptimizationPoint;
                    var same = true;
                    var ind = 0;

                    foreach (var parameter in this.parameters) {
                        Double cVal = x[ind];
                        Double pVal = Convert.ToDouble(dxOptimizationPoint.get_Value(parameter.ID));

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
                    var ind = 0;

                    pt = new DXOptimizationPoint();
                    foreach (Parameter parameter in this.parameters) {
                        Double val = x[ind];

                        //if (val < parameter.LowerBound || val > parameter.UpperBound)
                        //{
                        //    cost = double.MaxValue;
                        //    return _services.Stopped; 
                        //}
                        pt.put_Value(parameter.ID, val);
                        ind++;
                    }
                    this.services.CalculatePoint(pt);

                    if (pt.State == enumPointState.ePS_UpToDate) {
                        this.samples.Add(pt);
                    }
                }

                return pt;
            } catch {
                return null;
            }
        }

        public void EvaluateInequalities(Double[] x, out Double[] inequalities) {
            inequalities = null;

            if (this.constraints.Count == 0) {
                return;
            }

            inequalities = new Double[this.constraints.Count];
            try {
                IOptimizationPoint pt = this.ComputePoint(x);
                if (pt != null && pt.State == enumPointState.ePS_UpToDate) {
                    int ic = 0;
                    foreach (var c in this.constraints) {
                        inequalities[ic] = c.Compute(pt);
                        ic++;
                    }
                } else {
                    int ic = 0;
                    foreach (var c in this.constraints) {
                        inequalities[ic] = Double.MaxValue;
                        ic++;
                    }
                }
            } catch (Exception e) {
                int ic = 0;

                foreach (var c in this.constraints) {
                    inequalities[ic] = Double.MaxValue;
                    ic++;
                }
                Console.WriteLine(e);
            }
            return;
        }

        public bool EvaluateVector(Double[] x, out Double[] values) {
            values = new Double[this.objectives.Count];
            try {
                IOptimizationPoint pt = this.ComputePoint(x);
                if (pt != null && pt.State == enumPointState.ePS_UpToDate) {
                    this.services.PushHistoryPoint(pt);
                    int io = 0;

                    foreach (var o in this.objectives) {
                        values[io] = o.Compute(pt);
                        io++;
                    }
                    //foreach (Constraint c in _constraints)
                    //    cost = cost + c.Compute(pt);
                } else {
                    int io = 0;

                    foreach (var o in this.objectives) {
                        values[io] = Double.MaxValue;
                        io++;
                    }
                }
            } catch (Exception e) {
                int io = 0;
                foreach (var o in this.objectives) {
                    values[io] = Double.MaxValue;
                    io++;
                }
                Console.WriteLine(e);
            }
            return this.services.Stopped;
        }

        public void PushConvergenceData(Int32 iteration, Double metric) {
            IConvergenceData data = new DXConvergenceData();
            data.put_Value(0, iteration, metric);
            this.services.PushConvergenceData(data);
        }

        public Boolean Evaluate(Double[] x, out Double cost) {
            try {
                IOptimizationPoint pt = this.ComputePoint(x);
                if (pt != null && pt.State == enumPointState.ePS_UpToDate) {
                    this.services.PushHistoryPoint(pt);

                    cost = 0.0;
                    foreach (var o in this.objectives) {
                        cost = cost + o.Compute(pt);
                    }
                    //foreach (Constraint c in _constraints)
                    //    cost = cost + c.Compute(pt);
                } else {
                    cost = double.MaxValue;
                }
            } catch (Exception e) {
                cost = Double.MaxValue;
                Console.WriteLine(e);
            }
            return this.services.Stopped;
        }

        public void Run() {
            this.api.Log.WriteMessage("RUN");
            using (IServer server = new Server<DXServices>()) {
                DXServices services = server.GetServices() as DXServices;

                services.SetEvaluator(this);
                {
                    dynamic matlab = new Object();

                    try {
                        if (this.parameters.Count > 1) {
                            Double[,] values = matlab.GetVariable("x");
                            IEnumerator enume = values.GetEnumerator();

                            this.OutputResult(values);

                            foreach (IOptimizationPoint dxOptimizationPoint in this.samples) {
                                enume.Reset();
                                var same = true;

                                foreach (var parameter in this.parameters) {
                                    enume.MoveNext();
                                    var cVal = (Double)enume.Current;
                                    var pVal = Convert.ToDouble(dxOptimizationPoint.get_Value(parameter.ID));

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
                                    this.candidates.Add(dxOptimizationPoint);
                                    break;
                                }
                            }
                        } else {
                            Double values = matlab.GetVariable("x");

                            this.OutputResult(values);
                            foreach (IOptimizationPoint dxOptimizationPoint in this.samples) {
                                var same = true;
                                Parameter parameter = this.parameters[0];
                                var cVal = values;
                                var pVal = Convert.ToDouble(dxOptimizationPoint.get_Value(parameter.ID));

                                if (cVal != 0.0) {
                                    if (Math.Abs((cVal - pVal) / cVal) > 1e-10)
                                        same = false;
                                } else if (pVal != 0.0) {
                                    same = false;
                                }

                                if (same) {
                                    this.candidates.Add(dxOptimizationPoint);
                                    break;
                                }
                            }
                        }
                    } catch { }
                }

                services.Summary();
            }
        }

        protected void OutputResult(Object result) {
            Action<String> output = (message) => {
                Console.WriteLine(message);
                this.api.Log.WriteMessage(message);
            };
            var array = result as Array;

            if (array != null) {
                foreach (var obj in array) {
                    var messageString = String.Format("arr result = {0}", obj);

                    output(messageString);
                }
            } else {
                var messageString = String.Format("result = {0}", result);

                output(messageString);
            }
        }
    }
}