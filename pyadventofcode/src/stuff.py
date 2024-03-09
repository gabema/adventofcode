"""
Common functions and utilities used to complete advent of code challenges
"""
import io
import re
import time

def expected(func, fileName, expectedOutput) :
    start = time.perf_counter_ns()
    output = func(fileName)
    duration = (time.perf_counter_ns() - start) / 1000
    if output == expectedOutput :
        output = "(%dms) Success" % duration
    else :
        output = "(%dms) Failed! Got %s expected %s" % (duration, output, expectedOutput)
    return output

def readIntLinesArrays(fileName) :
    '''
    Given a file path parse the contents expected to contain lists of
    integers.
    Returns an iterator that returns lists of lists of ints.
    '''
    lines = readContentsByLine(fileName)
    lines = [l.strip('\n') for l in lines]    
    ints = []
    for l in lines :
        try :
            ints.append(int(l))
        except ValueError:
            if len(ints) > 0 :
                yield ints
                ints = []

    if len(ints) > 0 :
        yield ints

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

class Node:
    def __init__(self, data):
        self.data = data
        self.next = self
        self.prev = self

    def __str__(self) :
        return str(self.data)

    def __repr__(self):
        return self.data

class LinkedList:
    def __init__(self):
        self.head = None

    def __repr__(self):
        node = self.head
        return " -> ".join(self)

    def __getitem__(self, sentinel) :
        pass

    def __iter__(self) :
        node = self.head
        head = self.head
        if head != None :
            yield head
            node = head.next

        while(node != head) :
            node = node.next
            yield node


    def append(self, data) :
        node = Node(data)
        if self.head == None :
            self.head = node
        else :
            self.head.prev, node.prev, node.next = node, self.head.prev, self.head
