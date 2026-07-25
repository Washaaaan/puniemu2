import os

for file in os.listdir("."):
    if file.endswith("_def.txt"):
        print(file)