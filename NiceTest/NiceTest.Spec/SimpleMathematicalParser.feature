Feature: SimpleMathematicalParser
As part of the interview process, we require all candidates to submit a test. 
This test is about writing a custom mathematical parser to compute the value of an input string.

The parser must implement an order of precedence of left to right, brackets are used to explicitly 
denote precedence by grouping parts of an expression that should be evaluated first again left to right

The parser must recognise the input string and evaluate the expression.

Rules:

a = '+', b = '-', c = '*', d = '/', e = '(', f = ')'

Scenario Outline: Can parse
	Given I have a mathematical parser
	When I provide input '<Input>'
	Then the result should be '<Output>'
	Scenarios: 
	| Input              | Output |
	| 3a2c4              | 20     |
	| 32a2d2             | 17     |
	| 500a10b66c32       | 14208  |
	| 3ae4c66fb32        | 235    |
	| 3c4d2aee2a4c41fc4f | 990    |
