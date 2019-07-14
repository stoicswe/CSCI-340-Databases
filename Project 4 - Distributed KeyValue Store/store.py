import requests
from flask import Flask, send_file, render_template, request, abort, json, jsonify
#from flask_cors import CORS
import socket
import os
import time
import platform
import ast


# $ pip install pygtrie
import pygtrie as trie

app = Flask(__name__)
#CORS(app)

ryan = json.dumps({
    'name':     'Bryan Gates',
    'color':    'oranges',
    'ssn':      'N/A',
    'mmn':      'nope',
    'fear':     'break-force throws',
    'icecream': 'salted-carmel'
    })

store = trie.StringTrie(separator=':')
store["question:1"]  = "Favorite Veggie"

store["option:1:0"] = "Carrot"
store["option:1:1"] = "Broccoli"
store["option:1:2"] = "Pepper"

store["user:1"]     = ryan

store["answer:1:1"] = "Pepper"


#the vector clock
machines = []
with open("machines.txt", "r") as f:
    machines = [l.strip() for l in f.readlines()]

vc = {}
for m in machines:
    vc[m] = 0

storevc = dict() 
for k in store.keys():
    storevc[k] = vc.copy()

#Distributes change to other machines if they are older versions (so we need to send vc info and put/delete info).
#This will loop on every machine until sent version is no longer newer on any machine.
#function parameter examples: "put", "delete"
def replicate(function, args):
    for machineName in machines:
        #dont send to yourself
        if machineName != socket.gethostname():
            r = f"requests.{function}('http://{machineName}:5050/{function}', json={args})"
            eval(r)
    return "Replicated!"

#args = {"key":1, "value":12, machineName: "", vc: ""}
#requests.put('http://D09103:5000/put', json=args)

#we need correct code to retrieve vc and machine name from other computer's json
def getMachInfo(machine, args):
    if request.method == 'GET':
        args = request.args
    elif request.method == 'POST':
        args = request.get_json()
    
    if not "vc" or "machineName" in args:
        abort(404)

    ext_vc = args["vc"]
    machineName = args["machineName"]
    
    return (machineName, ext_vc)



@app.route("/")
def hello():
    with open("machines.txt", "r") as f:
        machines = [l.strip() for l in f.readlines()]
    return render_template("main.html",hostname = socket.gethostname(), vectorClock=vc, machines=machines, p = vc)

@app.route("/get", methods=["GET", "POST"])
def get():
    args = dict()
    if request.method == 'GET':
        args = request.args
    elif request.method == 'POST':
        args = request.get_json()
    
    if not "key" in args:
        abort(404)

    # Give a slow response:
    # time.sleep(1)
    key = args["key"]
    print("Post:", key)
    if not key in store:
        abort(404)
    return store[key]

@app.route("/get-prefix", methods=["GET", "POST"])
def get_prefix():
    args = dict()
    if request.method == 'GET':
        args = request.args
    elif request.method == 'POST':
        args = request.get_json()
    
    if not "key" in args:
        abort(404)

    # Give a slow response:
    # time.sleep(1)
    key = args["key"]
    print("Post:", key)
    if not store.has_subtrie(key):
        abort(404)
    return jsonify(dict(store.items(key)))

@app.route("/put", methods=["PUT"])
def put():
    # store value on machine
    args = request.get_json()
    if "vc" in args:
        print("Put | Key: {0} Value: {1} VC: {2}".format(args["key"], args["value"], args["vc"]))
        #work with the vector clock to see if the value is to be inserted.
        # recieved data Layout: args = {"key":1, "value":12, "vc": ""}
        # the value for VC can be made by simply sending args, with the value vc: str(vc), where the vc variable is the local vector clock
        # VC layput: vc = {'COMPNAME0': val, 'COMPNAME1': val, 'COMPNAME2': val, 'COMPNAME3': val}
        ext_vc = ast.literal_eval(str(args["vc"]))
        # now that we have the data seperated, we must perform causality checks
        # first check: make sure key exists
        if args["key"] in store.keys():
            past_vc = storevc[args["key"]]
            if(not vc_conflict(past_vc, ext_vc)):
                if(vc_before(past_vc, ext_vc)):
                    store[args["key"]] = args["value"]
                    # keeping record of the age of the data
                    storevc[args["key"]] = vc.copy()
                    vc_update(ext_vc)
                    args["vc"] = ext_vc
                    replicate("put", str(args))
            else:
                # what to do if the timestamps are the same?
                if(not store[args["key"]] == args["value"]):
                    # what to do if the values are different?
                    if(len(store[args["key"]]) < len(args["value"])):
                        store[args["key"]] = args["value"]
                        # keeping record of the age of the data
                        storevc[args["key"]] = vc.copy()
                        vc_update(ext_vc)
                        args["vc"] = ext_vc
                        replicate("put", str(args))
        else:
            store[args["key"]] = args["value"]
            # keeping record of the age of the data
            storevc[args["key"]] = vc.copy()
            vc_update(ext_vc)
            args["vc"] = ext_vc
            replicate("put", str(args))
    else:
        print("Put | Key: {0} Value: {1}".format(args["key"], args["value"]))
        store[args["key"]] = args["value"]
        # keeping record of the age of the data
        storevc[args["key"]] = vc.copy()
        ext_vc = None
        vc_update(ext_vc)
        args["vc"] = vc.copy()
        replicate("put", str(args))
    
    return "Thanks!"

@app.route("/delete", methods=["DELETE"])
def delete():
    #delete value on this machine
    args = request.get_json()
    print("Delete:", args["key"])
    key = args["key"]
    if key in store:
        del store[key]
        # keeping record of the age of the data
        storevc[args["key"]] = vc.copy()
        ext_vc = None
        # send update to other machines with vector clock of this machine
        vc_update(ext_vc)
        args["vc"] = vc.copy()
        replicate("delete", str(args))
    return "Deleted!"

def vc_before(t1,t2):
    return t1 != t2 and all([t1[k] <= t2[k] for k in t1.keys()])

def vc_conflict(t1, t2):
    if t1 == t2:
        return False
    return not(vc_before(t1,t2) or vc_before(t2,t1))

def vc_update(ext_vc):
    thisMachine = socket.gethostname()
    vc[thisMachine] = vc[thisMachine] + 1
    #if set(list(vc.keys())) != set(list(ext_vc.keys())):
    #    with open("machines.txt", "w") as f:
    #        for m in list(ext_vc.keys()):
    #            f.write(m + '\n')

    if(ext_vc != None):
        for vcl in ext_vc.keys():
            if(vcl != thisMachine):
                if(ext_vc[vcl] > vc[vcl]):
                    vc[vcl] = ext_vc[vcl]