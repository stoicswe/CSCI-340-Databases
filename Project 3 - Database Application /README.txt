DBOverflow Text-Based Application

Prerequisites:
---------------

This project is built using Dotnet Core, which can be installed from the following url:
https://dotnet.microsoft.com/download

You will need to install the NpgSQL Package after installing the dotnet core platform:
Homepage: https://www.npgsql.org
Install Package using NuGet: https://www.nuget.org/packages/Npgsql/

Before Running:
----------------

Before running the project, you must first open an SSH tunnel to the database machine on the
port: 5432.

In order to customize the connection parameters yourself, you'll need to change the constructor
for the DBOverflow Application object in the Program.cs file, located in this project.
The parameters for the DBOverflow object are as follows:

ServerName  = name of the server machine or IP address
Port        = port on which the server is hosting the database
UserName    = username for logging into the database
DBName      = the name of the database

Running the Project:
---------------------

In order to run the project, you will need to navigate to the directory location where the
project is located, and run the following command:

dotnet run

As the project is running, it will prompt you for a password to the DB. This is the password
used for logging into the DB for the chosen user you had customized in the Program.cs file.

Once you enter a password, the software will prompt for a user id, or a username. This is the
username for asking questions or submitting answers to questions in the DB. If you want to see
how the softwware looks when its running, take a look in the screenshots section of this readme.

Experience during the Project:
-------------------------------
Overall the project was very straight-forward and not too bad. Originally I had wanted to work
with Unity and make a video game that allowed users to browse the databasew in a very RPG-style
format. Unfortunately, I have not been able to do that due to complications I had faced with the
Unity platform, which was due to the fact of having very little experience with the game engine.

So, plan B was to create a text-based application that communicates with the database. Which,
overall, was developed quite smoothly. The hard part was working with the API for communicating
with the DB. The API only grabs a column at a time and so I was forced to divide my queries into
multiple, repeated queries, with each grabbing a specific column of the table I needed. This was
as bit annoying, and was the only situation that had slowed down the program slightly when retrieving
certain information, such as a list of all the questions.

The only feature I didn't get to add that I wanted to was listing the top users or listing the
users, ordered by the users scores. These scores would be calculated by upvote, downvote, and
a rating of activeness on the databse (where activeness would be a count of the number of questions
and/or answers that a particular user had written and submitted).


"SCREENSHOTS":
-----------------

Once you're signed in, the software will display a main menu:

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Currently signed in as: k4z1m1r
--------------------------------------------------------------------------------
1) Ask a question.
2) Answer a question.
3) View Top Questions.
4) View Random Questions.


ESC) Quit the application.
--------------------------------------------------------------------------------
>

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Everything from there is pretty straight forward, since options are either numbered or
have a letter associated with them. See the following menu listings:

ASK A QUESTION MENU:
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Signed in as: k4z1m1r
--------------------------------------------------------------------------------



Please enter your question (blank for exit):
>


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

ANSWER A QUESTION MENU:
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Currently signed in as: k4z1m1r
--------------------------------------------------------------------------------
31 | How do I get the answered questions ordered by the time it took to get an answer?
32 | Why do people hate on macOS so much? I find it to not be too bad...
37 | What does it take to get fired from a class on databases?
41 | I cant wait to see my girlfriends today?
44 | Can I have the database set to make noises everytime I query?
47 | How to code functionally like a pro?
49 | How to ace the Google interview?
52 | How can I find all the duplicate users in our database?
53 | How can I tell how efficient a query is?
54 | How do I add a column for votes on questions?
58 | What is the difference between a regular VIEW and a MATERIALIZED VIEW?
59 | What is the most important database skill for computer scientists who occasionally work with databases?
60 | How can a user interface for a database be modeled?
62 | What exactly is cost? I understand that lower is better but I don't fully understand what 'fetching single page in sequential manner' means.
63 | How does Filter work?
65 | How can understanding EXPLAIN better help us write better queries?
66 | What is 'Seq Scan'?
67 | Why is the time so different for explain and explain analyze?
68 | What exactly is the EXPLAIN commands anyway? Is it just a summary of the performance of the query, or a breakdown of what the query is?
69 | Why is there such a difference between expected and actual cost
73 | is there a way to track the amount of time it takes to call a query other then cost?
74 | What exactly is the integer in cost measuring?
75 | A question that I have regarding the reading would refer to the cost of the query and why is the time and resource usage different from seconds?
77 | Why was there a whale sound from next door?
80 | why didnt that work?
84 | Where am I redirected when I ask a question from this page?
85 | Did I successfully implement a redirect thing?
125 | Is it working now?
128 | What is the square root of 9?
133 | Testing
136 | Is this working?
139 | This is simply a test for Pandas
140 | This is simply a test for Pandas
147 | This is a test
148 | This is a test
149 | This is a test
150 | {}
151 | Test
153 | How far is the earth from the  sun
154 | What color are my pants?
156 | 
--------------------------------------------------------------------------------
Enter a question id to view the question or 'quit' to exit.
>

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

VIEW TOP QUESTIONS:
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Currently signed in as: k4z1m1r
--------------------------------------------------------------------------------
98 | Am I an expert web developer yet?
144 | How do I answer a question using sql?
2 | What is the difference between text and strings?
8 | How do I insert a new entry into a table?
107 | How is everyone doing on their projects?
112 | According to all known laws of aviation, there is no way a bee should be able to fly. Its wings are too small to get its fat little body off the groun...
34 | Are SQL Databases easily hackable? How secure are they?
71 | What does 'fetching single page in sequential manner' mean as a unit? Is this a measurement of memory taken up or time it takes or something else?
122 | bla
50 | How to write clean and concise SQL? Is it even possible?
130 | Is it finally working?
152 | MOMMY LOOK I DID A DATABASE THING
96 | What is the best way to create a ui?
12 | How to make a waterbottle feel loved?
30 | Can I insert into a view?
95 | What is the best framework for making a database-backed website?
137 | Is this working?
102 | How do you make a webpage look nice?
70 | What is a page?
54 | How do I add a column for votes on questions?
74 | What exactly is the integer in cost measuring?
64 | What is width?
87 | Can I do this with pandas?
10 | Where is the bathroom?
90 | Did this actually work?
--------------------------------------------------------------------------------
Enter a question id to view the question or 'quit' to exit.
>
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

VIEW RANDOM QUESTIONS (basically view all questions):
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Currently signed in as: k4z1m1r
--------------------------------------------------------------------------------
2 | What is the difference between text and strings?
3 | How do I add a question to a database?
4 | What is a Ryan Yates?
5 | What is life? Why do we exist?
........................................................
152 | MOMMY LOOK I DID A DATABASE THING
153 | How far is the earth from the  sun
154 | What color are my pants?
155 | Sample Question HAHA
156 | 
--------------------------------------------------------------------------------
Enter a question id to view the question or 'quit' to exit.
>
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''