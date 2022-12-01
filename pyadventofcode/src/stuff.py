"""
Common functions and utilities used to complete advent of code challenges
"""
import io
import re

def readContents(fileName) :
    """
    Read the entire contents of the specified file and return it as a string
    """
    file = io.open(fileName)
    contents = file.read()
    file.close()
    return contents

def readContentsByLine(fileName) :
    """
    reads in each line and puts in a list
    """
    file = io.open(fileName)
    contents = file.readlines()
    file.close()
    return contents

def readStructuredContents(fileName, regExp, flags = None) :
    contents = readContents(fileName)
    return re.findall(regExp, contents, flags)