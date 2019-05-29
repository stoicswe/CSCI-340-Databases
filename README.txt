README - Nathaniel Bunch

Group: Ryan Ozzello, Ryan Nickelson, Teresa Soley

Solution:

We developed a solution based on vector clocks. The program utilizes said vector clocks to
track the changes to the key value store and replicate those changes across a distributed
system.

Difficulties:

The primary difficulty was not only developing the vector clock, but also testing to make sure
the clocks were iterating peroperly. We dont know if they iterate correctly, however, values
are entered correctly into the distributed key-value store.

Missing Features:

Some of the missing features that we didnt have time to implement, was data recovery, in the case
of a server shutdown or power outage. We began developing the solution, but did not make it very
far before the powers of sleepiness took over us.

How to use:

In order to use the software, it must be distributed onto each machine that is to be used. Each machine
name or IP address must be added into a file wihtin the project directory with the name machines.txt. When
the software runs, the machines will automatically look for the names listed in the machines.txt file.

The software will also open a webserver on port 5050 for each machine, which can be visited by a webrowser.
Entries to the key-value store will be distributed to all the servers running the software automatically.

In order to run the sopftware, you must naviage to the cloned project directory and either run: run-quiz-win.bat for
running on windows or the run-quiz.sh for running on linux or macOS.