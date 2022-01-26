// ModularCalculationMain.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>

#include "../ModularUnitTests/ModularUnitTests.cpp"


int main()
{
	ModularUnitTests::ModularUnitTests mut;
	for (std::function<void()> f : mut.funcarray)
	{
		f();
		if (!Assert::Condition)
			throw std::runtime_error("Test failed");
	}
}

