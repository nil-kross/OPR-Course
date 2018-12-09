using Ansys.DesignXplorer.API.Common;
using Ansys.DesignXplorer.API.Optimization;
using Ansys.DesignXplorer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ansys {
    public class GenAlOptimizer : IOptimizationMethod {
        public GenAlOptimizer(Int32 generations) {
            ;
        }

        public Object get_Setting(String bsSetting) {
            throw new NotImplementedException();
        }

        public void put_Setting(String bsSetting, Object vntVal) {
            throw new NotImplementedException();
        }

        public void AddDoubleVariable(String bsVariableID, Double dblLowerBound, Double dblUpperBound, Double dblInitialValue) {
            throw new NotImplementedException();
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

        public Boolean CanRun(out String bsErrorMessage) {
            throw new NotImplementedException();
        }

        public void Run() {
            throw new NotImplementedException();
        }

        public void AddCustomVariableProperty(String bsVariableID, String bsPropertyKey, Object vntPropertyValue) {
            throw new NotImplementedException();
        }

        public void AddObjective(String bsVariableID, enumGoalType type, Double dblTargetValue, Double dblImportance) {
            throw new NotImplementedException();
        }

        public void AddConstraint(String bsVariableID, enumConstraintType type, Object vntConstraintValue1, Object vntConstraintValue2, Double dblImportance, Boolean vbStrictConstraint) {
            throw new NotImplementedException();
        }

        public void AddCustomObjectiveProperty(String bsVariableID, String bsPropertyKey, Object vntPropertyValue) {
            throw new NotImplementedException();
        }

        public void AddCustomConstraintProperty(String bsVariableID, String bsPropertyKey, Object vntPropertyValue) {
            throw new NotImplementedException();
        }

        public IOptimizationServices Services { set => throw new NotImplementedException(); }

        public enumPostProcessingType PostProcessingTypes => throw new NotImplementedException();

        public Ans.ComponentSystem.IListCpp Candidates => throw new NotImplementedException();

        public Ans.ComponentSystem.IListCpp Samples => throw new NotImplementedException();
    }
}