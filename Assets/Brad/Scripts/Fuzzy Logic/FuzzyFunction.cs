using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class FuzzyFunction
* 
* contains two function references, one evaluates the input and returns a score
* the other executes an action
* 
* author: Daniel Witt, Bradley Booth, Academy of Interactive Entertainment, 2017
*/
public class FuzzyFunction
{
    //define function reference types
    public delegate float EvaluationFunc(BaseInput baseInput);
    public delegate void ExecutionFunc(BaseInput baseInput);

    //two function references to complete the fuzzy function
    public EvaluationFunc evaluation = null;
    public ExecutionFunc execution = null;
}
