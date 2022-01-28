// ModularCalculationMain.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>


#ifndef _WIN32
#include "../ModularUnitTests/ModularUnitTests.h"
#endif

int main()
{
#ifndef _WIN32
	ModularUnitTests::ModularUnitTests mut;
	int pass = -1;
	int fail = 0;
	for (std::function<void()> f : mut.funcarray)
	{
		f();
		if (Assert::Condition)
			pass++;
		else
		{ 
			fail++;
			std::cout << "Failed Test, Pass: " << pass << " Fail: " << fail << std::endl;
		}
	}
	std::cout << "Pass: " << pass << std::endl << "Fail: " << fail << std::endl;
	if (fail > 0)
		return 1;
	else
		return 0;
#endif
}

