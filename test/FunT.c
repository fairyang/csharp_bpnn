/****FunT****/

#include <stdio.h>

#include <stdlib.h>

#include <windows.h>

#include "dll.h"

DLLIMPORT __declspec(dllexport) int FunT(int a,int b)

{

       int c =a+b;

       printf("%d",c);

       //system("PAUSE");

       return(c);

}
