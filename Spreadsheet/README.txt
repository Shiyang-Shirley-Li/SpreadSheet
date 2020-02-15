Author:     [Shirley(Shiyang) Li]
Partner:    [None]
Date:       [2/10/2020]
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment Five - Spreadsheet Model
Copyright:  CS 3500 and [Shirley(Shiyang) Li] - This work may not be copied for use in Academic Coursework.

Hours Estimated/Worked         Assignment                                         Note
         12    /   18      - Assignment 1 - Formula Evaluator                   Spent 6 extra hours debugging and achieving Good Software Practice, otherwise good.
         18    /   17      - Assignment 2 - Dependency Graph                    Spent 1 hours less.
		 20	   /   15      - Assignment 3 - Refactoring the FormulaEvaluator    Spent 5 hours less by coding, commenting and testing at the same time.
		 16	   /   12      - Assignment 4 - Onward to a Spreadsheet             Spent 4 hours less by coding, commenting and testing at the same time.
		 16	   /   20      - Assignment 5 - Spreadsheet Model					Spent 4 extra hours trying to understand the instructions, debugging.

Assignment Five - Spreadsheet Model   2/10/2020 - 2/14/2020
1. Comments to Evaluators:
	1. Have problem with getDirectDependents method. I do not know why the dependency graph will reset after leaving the setContentsOfCell even if I add dependency 
	to denpendency graph in setCellContents method. So I commet two failed tests.

2. Assignment Specific Topics:
	1. I estimate that I need 16 hours for this project;
	2. Exactly, I spent 20 hours. I spent about 10 hours making progress toward completion, about 5 hours debugging, about 2 hours learning instruction, tools and 
	techiniques, and about 3 hours to fulfill the requirement for Good Software Practice. 
	3. I think I estimated my time close to what I really needed. What I have learnt about my time management is that Black testing is super important for decreasing 
	the time. Also, regression testing can help us find our bugs right away, then we can fix it before having problems elsewhere we use it, which can also save a large 
	amount of time.

3. Consulted Peers:
	None

4. References:
	None

5. Examples of Good Software Practice:
	1. isVarialbe(string variable) helper method, which reduces the reuse of code;
	2. setCellContentsHelper(string name, object newContent), which avoid code repeating;
	3. exceptionHelper(string name), which avoid code repeating.
	








Assignment One - Formula Evaluator   01/11/2020 - 01/17/2020
1. Comments to Evaluators:
	1. I combined the code for both the token is an integer and a variable to avoid reusing code by looking up 
	the variable at the very beginning and changing it into an integer;
	2. I combined the code for "If + or - or / or * is at the top of the operator stack, pop the value stack twice 
	and the operator stack once" case as a helper method to avoid reusing code;

2. Assignment Specific Topics
	1. I estimate that I need 12 hours for this project;
	2. Exactly, I spent 18 hours. I spent about 6 hours making progress toward completion, about 8 hours debugging, 
	about 30 minutes learning tools and techiniques, and about 3 hours to fulfill the requirement for Good Software Practice.

3. Consulted Peers:
	None.

4. References:
	None.


Assignment TWO - Dependency Graph   01/20/2020 - 01/24/2020
1. Comments to Evaluators:

2. Assignment Specific Topics
	1. I estimate that I need 18 hours for this project;
	2. Exactly, I spent 17 hours. I spent 5 hours making progress toward completion, 8 hours debugging and adding test cases, 3 hours commenting 
	and making Good Software Practice, and an hour learning API.

3. Consulted Peers:
	None.

4. References:
	None.

Assignment Three - Refactoring the FormulaEvaluator   01/27/2020 - 01/31/2020
1. Comments to Evaluators:
	1. Using Extensions and private helper method to avoid repeating of code.

2. Assignment Specific Topics
	1. I estimate that I need 20 hours for this project;
	2. Exactly, I spent 15 hours. I spent 9 hours making progress toward completion, 6 hours debugging and adding test cases,  30 minutes commenting 
	and making Good Software Practice, and 30 minutes learning API.

3. Consulted Peers:
	None.

4. References:
	None.


Assignment Four - Onward to a Spreadsheet   02/03/2020 - 02/07/2020
1. Comments to Evaluators:
	I wrote black tests before implementing, and I found it really helps a lot.

2. Assignment Specific Topics:
	1. I estimate that I need 16 hours for this project;
	2. Exactly, I spent 12 hours. I spent 6 hours making progress toward completion, 3 hours debugging and adding test cases,  30 minutes commenting 
	and 2 hours making Good Software Practice, and 30 minutes learning API.

3. Consulted Peers:
	None

4. References:
	None

5. Examples of Good Software Practice:
	1. In Spreadsheet, I used two helper methods to avoid repeated code. One is the SetCellContentsHelper(string name, object newContent) helper method to reduce the
	   repeated code in the three overloaded setCellContents method. The other is the exceptionHelper(String name) helper method to reduce the repeated use of checking
	   validity of the name.