// ModularCalculationMain.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>

#ifndef _WIN32
#include "../ModularUnitTests/ModularUnitTests.cpp"
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
			fail++;
	}
	if (fail > 0)
		return 1;
	else
		return 0;
#endif
}

