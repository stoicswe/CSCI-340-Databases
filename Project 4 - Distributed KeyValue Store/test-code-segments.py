# This file is for running local code tests.
from flask import Flask, send_file, render_template, request, abort, json, jsonify
import pygtrie as trie

with open("test.txt", "w") as f:
  for i in range(10):
    f.write(str(i) + '\n')