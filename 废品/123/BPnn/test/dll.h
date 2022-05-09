#ifndef _DLL_H_
#define _DLL_H_

#if BUILDING_DLL
#define DLLIMPORT __declspec(dllexport)
#else
#define DLLIMPORT __declspec(dllimport)
#endif

DLLIMPORT __declspec(dllexport) double Hello(double a1,double a2,double a3);
DLLIMPORT __declspec(dllexport) int FunT1(int a,int b);
DLLIMPORT __declspec(dllexport) int FunT(int a,int b);

#endif
