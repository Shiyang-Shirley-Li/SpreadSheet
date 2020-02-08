Author:     [Shirley(Shiyang) Li]
Partner:    [None]
Date:       [2/3/2020]
Course:     CS 3500, University of Utah, School of Computing
Assignment: Assignment Four - Onward to a Spreadsheet
Copyright:  CS 3500 and [Shirley(Shiyang) Li] - This work may not be copied for use in Academic Coursework.

Hours Estimated/Worked         Assignment                                         Note
         12    /   18      - Assignment 1 - Formula Evaluator                   Spent 6 extra hours debugging and achieving Good Software Practice, otherwise good.
         18    /   17      - Assignment 2 - Dependency Graph                    Spent 1 hours less.
		 20	   /   15      - Assignment 3 - Refactoring the FormulaEvaluator    Spent 5 hours less by coding, commenting and testing at the same time.
		 16	   /   12      - Assignment 4 - Onward to a Spreadsheet             Spent 4 hours less by coding, commenting and testing at the same time.

Assignment Four - Onward to a Spreadsheet   2/3/2020 - 2/7/2020
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