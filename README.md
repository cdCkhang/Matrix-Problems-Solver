<h1>Matrix Problems Solver Web Application using Maple Module</h1>
<h2>Table of contents</h2>
<ul>
<li><a href="#section1">Overview</a></li>
<li><a href="#section2">Technologies stack</a></li>
<li><a href="#section3">How it works</a></li>
<li><a href="#section3b"> Folders info </a></li>
<li><a href="#section4">Prequisites</a></li>
<li><a href="#section5">Video demonstration</a></li>
</ul>

<h2 id="section1">Overview:</h2>

This is a simple web application that can solve some basic matrix operations like: Addition, Subtraction and Multiplication.  
It also gives out detailed steps by steps guide on how to perform the operations between two matrices.  
The main component that solves the matrix problem is written and packaged using Maple Software.

<h2 id="section2"> Technologies used: </h2>

+ Front-end & Back-end: Written in C# using the ASP.NET Core framework. 
+ Problem solving module: Written, compiled and packaged by Maple Software. 


<h2 id="section3"> How it works: </h2>
<ol>
<li> First the user will input matrix's dimensions like rows and columns.</li>
<li>After that select an operation and click the button to solve.</li>
<li>Data from the user's input will be handled and put into a set of commands along with the precompiled module.</li>
<li> The instructions are given to Maple commands execution program to solve the problem.</li>
<li> The result will be an HTML file in the result folder, which the server can read to display the output to the webpage.</li>
</ol>

<h2 id="section3b"> Directories: </h2>
<ul>

<li>BatFiles:  These are the files made during the solving process. It is a collection executable instructions feed to the Windows OS in form of .bat files.</li>
<li>Data:  User's input and function calls defined in the built module will be stored here.</li>
<li>KB-IE:  Built module, this is the core of the problem solving abilities.</li>
<li>Solved:  Collections of .html file to display the results to the webpage.</li>
<li>module_raw:  Source code of the maple solving module.</li>

</ul>


<h2 id="section4"> Perequisites: </h2>

+ <a href="https://www.maplesoft.com/">Visual Studio (2019/2022)</a>: Code environment, Host the web application.
+ <a href="https://www.maplesoft.com/">Maple Software</a>: Execute the given instructions from the backend of the server.
+ <a> Maple Player </a>: Lighter version of the Maple Sofware. It can view and do basic edits to the original module's source code.

<h2 id="section5">Demo:</h2>
https://github.com/cdCkhang/Matrix-Problems-Solver/blob/main/assets/web-demo.gif

You are welcoming to use or modify any part of the source code.



