using System;
using System.Collections.Generic;
using Ans.DesignXplorer.CommonMatlabApplication;

namespace Ans.DesignXplorer.MatlabOptimizer {
    public class gaOptimizer : baseOptimizer {
        private LogLevel _logLevel;

        // Population settings

        // Population Type: String describing the data type of the population
        // - not supported yet

        // Size of the Population
        private int _populationSize = 20;

        // Creation function: the function that creates the initial population
        // - not supported yet

        // Initial Population: Initial population used to seed the genetic algorithm
        // - not supported yet

        // Initial Socres: Initial scores used to determine fitness
        // - not supported yet

        // Population Initial Range: Matrix or vector specifying the range of the individuals in the initial population
        // - not supported yet

        // Fitness Scaling Function: Handle to the function that scales the values of the fitness function
        private string _fitnessScalingFunction = "@fitscalingrank";

        // Quantity (when Fitness Scaling Function is Top)
        private double _topquantity = 0.4;

        // Maximum Survival Rate (when Fitness Scaling Function is Shift linear):  
        // Shift linear scaling scales the raw scores so that the expectation of the fittest individual is equal to a constant multiplied by the average score. 
        // You specify the constant in the Max survival rate field
        private int _maxSurvivalRate = 2;

        // Selection Function: Handle to the function that selects parents of crossover and mutation children
        private string _selectionFunction = "@selectionstochunif";

        // Tournament Size (when Selection Function is Tournament):
        // Tournament selection chooses each parent by choosing Tournament size players at random and then choosing the best individual out of that set to be a parent
        private int _tournamentSize = 4;

        // Elite Count (for Reproduction): Positive integer specifying how many individuals in the current generation are guaranteed to survive to the next generation
        private int _eliteCount = 1;

        // Crossover Fraction: The fraction of the population at the next generation, not including elite children, that is created by the crossover function
        private double _crossoverFraction = 0.8;

        // Mutation Function: Handle to the function that produces mutation children
        private string _mutationFunction = "@mutationadaptfeasible";

        // Scale (when Mutation Function is Gaussian): determines the standard deviation at the first generation
        private double _mutationgaussianscale = 1.0;

        // Shrink (when Mutation Function is Gaussian): controls how the standard deviation shrinks as generations go by
        private double _mutationgaussianshrink = 1.0;

        // Rate (when Mutation Function is Uniform):
        // Uniform mutation is a two-step process. 
        // First, the algorithm selects a fraction of the vector entries of an individual for mutation, where each entry has a probability Rate of being mutated. 
        // In the second step, the algorithm replaces each selected entry by a random number selected uniformly from the range for that entry.
        private double _mutationuniformrate = 0.01;

        // CrossOver Function: Handle to the function that the algorithm uses to create crossover children
        private string _crossoverFunction = "@crossoverscattered";

        // Ratio (when Crossover Function is Intermediate)
        private double _intermediateratio = 1.0;

        // Ratio (when Crossover Function is Heuristic)
        private double _heuristicratio = 1.2;

        // Migration Direction
        private string _migrationdirection = "forward";

        // Migration Fraction: Scalar between 0 and 1 specifying the fraction of individuals in each subpopulation that migrates to a different subpopulation
        private double _migrationfraction = 0.2;

        // Migration Interval: Positive integer specifying the number of generations that take place between migrations of individuals between subpopulations
        private int _migrationinterval = 20;

        // Initial Penalty: specifies an initial valueto be used by the algorithm. Initial penalty mustbe greater than or equal to 1.
        private double _initialpenalty = 10;

        // Penalty Factor: increases the penalty parameterwhen the problem is not solved to required accuracy and constraintsare not satisfied. Penalty factor must be greaterthan 1.
        private double _penaltyfactor = 100;

        // Hybrid Function: Handle to a function that continues the optimization after ga terminates
        private string _hybridfunction = "[]";

        // Hybrid function options
        private string _hybridfunctionoptions = "[]";

        // Generations: Positive integer specifying the maximum number of iterations before the algorithm halts
        private int _generations = 100;

        // FitnessLimit: Scalar. If the fitness function attains the value of FitnessLimit, the algorithm halts.
        private double _fitnesslimit = Double.MinValue;

        // StallGenLimit: Positive integer. The algorithm stops if the weighted average relative change in the best fitness function value over StallGenLimit generations is less than or equal to TolFun.
        private int _stallgenlimit = 50;

        // TolFun: Positive scalar. The algorithm stops if the weighted average relative change in the best fitness function value over StallGenLimit generations is less than or equal to TolFun.
        private double _tolfun = 1.0e-6;

        // TolCon: Positive scalar. TolCon is used to determine the feasibility with respect to nonlinear constraints. 
        private double _tolcon = 1.0e-6;

        // Display: Level of display to specify the amount of information displayed when you run the algorithm, and the type of exit message (default or detailed).
        private string _display = "off";

        private string _message = string.Empty;

        private readonly Dictionary<string, string> _scalingFunctions = new Dictionary<string, string>()
        {
            {"Rank","@fitscalingrank"},
            {"Proportional","@fitscalingprop"},
            {"Top","{@fitscalingtop,topquantity}"},
            {"Shift linear","{@fitscalingshiftlinear,shiftlinearrate}"}
        };

        private readonly Dictionary<string, string> _selectionFunctions = new Dictionary<string, string>()
        {
            {"Stochastic uniform","@selectionstochunif"},
            {"Remainder","@selectionremainder"},
            {"Uniform","@selectionuniform"},
            {"Roulette","@selectionroulette"},
            {"Tournament","{@selectiontournament,tournamentsize}"}
        };

        private readonly Dictionary<string, string> _mutationFunctions = new Dictionary<string, string>()
        {
            {"Adaptive Feasible","@mutationadaptfeasible"},
            {"Gaussian","{@mutationgaussian,mutationgaussianscale,mutationgaussianshrink}"},
            {"Uniform","{@mutationuniform,mutationuniformrate}"}
        };

        private readonly Dictionary<string, string> _crossoverFunctions = new Dictionary<string, string>()
        {
            {"Scattered","@crossoverscattered"},
            {"Single point","@crossoversinglepoint"},
            {"Two point","@crossovertwopoint"},
            {"Intermediate","{@crossoverintermediate,intermediateratio}"},
            {"Heuristic","{@crossoverheuristic,heuristicratio}"},
            {"Arithmetic","@crossoverarithmetic"}
        };

        private readonly Dictionary<string, string> _hybridFunctions = new Dictionary<string, string>()
        {
            {"None","[]"},
            {"fminsearch","{@fminsearch,hybridopts} "},
            {"patternsearch","{@patternsearch,hybridopts}"},
            {"fminunc","{@fminunc,hybridopts}"},
            {"fmincon","{@fmincon,hybridopts}"}
        };

        private readonly Dictionary<string, string> _displaytypes = new Dictionary<string, string>()
        {
            {"off","off"},
            {"final","final"},
            {"diagnose","diagnose"},
            {"iterative","iter"},
        };

        public override LogLevel GetLogLevel() {
            return _logLevel;
        }

        public override object get_Setting(string bsSetting) {
            switch (bsSetting) {
                case "LogLevel":
                return _logLevel.ToString();
                case "PopulationSize":
                return _populationSize;
                case "FitnessScalingFcn":
                return _fitnessScalingFunction;
                case "TopQuantity":
                return _topquantity;
                case "MaxSurvivalRate":
                return _maxSurvivalRate;
                case "SelectionFcn":
                return _selectionFunction;
                case "TournamentSize":
                return _tournamentSize;
                case "EliteCount":
                return _eliteCount;
                case "CrossoverFraction":
                return _crossoverFraction;
                case "MutationFcn":
                return _mutationFunction;
                case "MutationGaussianScale":
                return _mutationgaussianscale;
                case "MutationGaussianShrink":
                return _mutationgaussianshrink;
                case "MutationUniformRate":
                return _mutationuniformrate;
                case "CrossoverFcn":
                return _crossoverFunction;
                case "IntermediateRatio":
                return _intermediateratio;
                case "HeuristicRatio":
                return _heuristicratio;
                case "MigrationDirection":
                return _migrationdirection;
                case "MigrationFraction":
                return _migrationfraction;
                case "MigrationInterval":
                return _migrationinterval;
                case "InitialPenalty":
                return _initialpenalty;
                case "PenaltyFactor":
                return _penaltyfactor;
                case "HybridFcn":
                return _hybridfunction;
                case "HybridFcnOptions":
                return _hybridfunctionoptions;
                case "Generations":
                return _generations;
                case "FitnessLimit":
                return _fitnesslimit;
                case "StallGenLimit":
                return _stallgenlimit;
                case "TolFun":
                return _tolfun;
                case "TolCon":
                return _tolcon;
                case "Display":
                return _display;
                case "OptimizationStatus":
                return _message;
            }
            throw new KeyNotFoundException();
        }

        public override void put_Setting(string bsSetting, object pvntVal) {
            string func;
            switch (bsSetting) {
                case "LogLevel":
                LogLevel level;
                if (Enum.TryParse<LogLevel>(pvntVal.ToString(), false, out level)) {
                    _logLevel = level;
                } else {
                    _logLevel = LogLevel.None;
                }
                break;
                case "PopulationSize":
                _populationSize = (int)pvntVal;
                break;
                case "FitnessScalingFcn":
                _fitnessScalingFunction = _scalingFunctions.TryGetValue(pvntVal.ToString(), out func) ? func : "@fitscalingrank";
                break;
                case "TopQuantity":
                _topquantity = (double)pvntVal;
                break;
                case "MaxSurvivalRate":
                _maxSurvivalRate = (int)pvntVal;
                break;
                case "SelectionFcn":
                _selectionFunction = _selectionFunctions.TryGetValue(pvntVal.ToString(), out func) ? func : "@selectionstochunif";
                break;
                case "TournamentSize":
                _tournamentSize = (int)pvntVal;
                break;
                case "EliteCount":
                _eliteCount = (int)pvntVal;
                break;
                case "CrossoverFraction":
                _crossoverFraction = (double)pvntVal;
                break;
                case "MutationFcn":
                _mutationFunction = _mutationFunctions.TryGetValue(pvntVal.ToString(), out func) ? func : "@mutationgaussian";
                break;
                case "MutationGaussianScale":
                _mutationgaussianscale = (double)pvntVal;
                break;
                case "MutationGaussianShrink":
                _mutationgaussianshrink = (double)pvntVal;
                break;
                case "MutationUniformRate":
                _mutationuniformrate = (double)pvntVal;
                break;
                case "CrossoverFcn":
                _crossoverFunction = _crossoverFunctions.TryGetValue(pvntVal.ToString(), out func) ? func : "@crossoverscattered";
                break;
                case "IntermediateRatio":
                _intermediateratio = (double)pvntVal;
                break;
                case "HeuristicRatio":
                _heuristicratio = (double)pvntVal;
                break;
                case "MigrationDirection":
                _migrationdirection = pvntVal.ToString();
                break;
                case "MigrationFraction":
                _migrationfraction = (double)pvntVal;
                break;
                case "MigrationInterval":
                _migrationinterval = (int)pvntVal;
                break;
                case "InitialPenalty":
                _initialpenalty = (double)pvntVal;
                break;
                case "PenaltyFactor":
                _penaltyfactor = (double)pvntVal;
                break;
                case "HybridFcn":
                _hybridfunction = _hybridFunctions.TryGetValue(pvntVal.ToString(), out func) ? func : "[]";
                break;
                case "HybridFcnOptions":
                _hybridfunctionoptions = pvntVal.ToString().Length == 0 ? "[]" : pvntVal.ToString();
                break;
                case "Generations":
                _generations = (int)pvntVal;
                break;
                case "FitnessLimit":
                _fitnesslimit = (double)pvntVal;
                break;
                case "StallGenLimit":
                _stallgenlimit = (int)pvntVal;
                break;
                case "TolFun":
                _tolfun = (double)pvntVal;
                break;
                case "TolCon":
                _tolcon = (double)pvntVal;
                break;
                case "Display":
                _display = _displaytypes.TryGetValue(pvntVal.ToString(), out func) ? func : "off";
                break;
            }
        }

        public override bool PushOptions(MatlabApplication matlab) {
            string command = string.Format("[lic errmsg]=license('checkout','gads_toolbox');");
            matlab.Execute(command);
            var errmsg = matlab.GetVariable("errmsg");
            string msg = (errmsg == null) ? string.Empty : errmsg.ToString();
            if (!string.IsNullOrEmpty(msg)) {
                _message = msg;
                return false;
            }

            // definition of anonymous function: this function forwards Output Function values from matlab to DX
            command = string.Format("myoutputfunction = @(iterStep,iterValue,flag) optimconvergence.OutputDelegate(iterStep,iterValue,flag);");
            matlab.Execute(command);

            switch (_fitnessScalingFunction) {
                case "{@fitscalingtop,topquantity}":
                command = string.Format("topquantity = {0};", _topquantity.ToString(System.Globalization.CultureInfo.InvariantCulture));
                matlab.Execute(command);
                break;
                case "{@fitscalingshiftlinear,shiftlinearrate}":
                command = string.Format("shiftlinearrate = {0};", _maxSurvivalRate.ToString(System.Globalization.CultureInfo.InvariantCulture));
                matlab.Execute(command);
                break;
            }

            if (_selectionFunction == "{@selectiontournament,tournamentsize}") {
                command = string.Format("tournamentsize = {0};", _tournamentSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
                matlab.Execute(command);
            }

            switch (_mutationFunction) {
                case "{@mutationgaussian,mutationgaussianscale,mutationgaussianshrink}":
                command = string.Format("mutationgaussianscale = {0}; mutationgaussianshrink = {1};", _mutationgaussianscale.ToString(System.Globalization.CultureInfo.InvariantCulture), _mutationgaussianshrink.ToString(System.Globalization.CultureInfo.InvariantCulture));
                matlab.Execute(command);
                break;
                case "{@mutationuniform,mutationuniformrate}":
                command = string.Format("mutationuniformrate = {0};", _mutationuniformrate.ToString(System.Globalization.CultureInfo.InvariantCulture));
                matlab.Execute(command);
                break;
            }

            switch (_crossoverFunction) {
                case "{@crossoverintermediate,intermediateratio}":
                command = string.Format("intermediateratio = {0}", _intermediateratio.ToString(System.Globalization.CultureInfo.InvariantCulture));
                matlab.Execute(command);
                break;
                case "{@crossoverheuristic,heuristicratio}":
                command = string.Format("heuristicratio = {0};", _heuristicratio.ToString(System.Globalization.CultureInfo.InvariantCulture));
                matlab.Execute(command);
                break;
            }

            if (_hybridfunction != "[]") {
                command = string.Format("[lic errmsg]=license('checkout','optimization_toolbox');");
                matlab.Execute(command);
                errmsg = matlab.GetVariable("errmsg");
                msg = (errmsg == null) ? string.Empty : errmsg.ToString();
                if (!string.IsNullOrEmpty(msg)) {
                    _message = msg;
                    return false;
                }

                command = string.Format("hybridopts = {0};", _hybridfunctionoptions);
                matlab.Execute(command);
            }

            command = string.Format("options = gaoptimset(" +
                                    "'PopulationSize',{0}," +
                                    "'FitnessScalingFcn',{1}," +
                                    "'SelectionFcn',{2}," +
                                    "'EliteCount',{3}," +
                                    "'CrossoverFraction',{4}," +
                                    "'MutationFcn',{5}," +
                                    "'CrossoverFcn',{6}," +
                                    "'MigrationDirection','{7}'," +
                                    "'MigrationFraction',{8}," +
                                    "'MigrationInterval',{9}," +
                                    "'InitialPenalty',{10}," +
                                    "'PenaltyFactor',{11}," +
                                    "'HybridFcn',{12}," +
                                    "'Generations',{13}," +
                                    "'FitnessLimit',{14}," +
                                    "'StallGenLimit',{15}," +
                                    "'TolFun',{16}," +
                                    "'TolCon',{17}," +
                                    "'Display','{18}'," +
                                    "'OutputFcn', @(options,state,flag)gaplotfunction_min(options,state,flag,myoutputfunction))",
                                    _populationSize.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _fitnessScalingFunction,
                                    _selectionFunction,
                                    _eliteCount.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _crossoverFraction.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _mutationFunction,
                                    _crossoverFunction,
                                    _migrationdirection,
                                    _migrationfraction.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _migrationinterval.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _initialpenalty.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _penaltyfactor.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _hybridfunction,
                                    _generations.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _fitnesslimit.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _stallgenlimit.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _tolfun.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _tolcon.ToString(System.Globalization.CultureInfo.InvariantCulture),
                                    _display
                                    );

            matlab.Execute(command);
            return true;
        }

        public override void RunOptimizer(MatlabApplication matlab, string startingpoint, string lowerbounds, string upperbounds) {

            string command = string.Format("[x fval exitflag, output] = ga(fun, {0}, [], [], [], [], {1}, {2}, nonlcon, options)",
                                                _parameters.Count, lowerbounds, upperbounds);
            matlab.Execute(command);
        }

        public override void GetResults(MatlabApplication matlab) {
            try {
                matlab.Execute("status = output.message");
                _message = matlab.GetVariable("status").ToString();
            } catch (Exception) {

                throw;
            }
        }
    }
} 