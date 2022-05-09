#include "dll.h"

#include <windows.h>

#include <string.h>

DLLIMPORT __declspec(dllexport) int FunT1(int a,int b)

{

    char output[1000];

    char st[20];

    int c=a+b;

    _itoa(c, st, 10);

    strcpy(output,"this sum of integer is :");

    strcat(output,st);

    MessageBox(NULL,output,"show console output in MessageBox",MB_OK);

    return (0);
    

}
