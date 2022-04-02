#include <iostream>

#include <windows.h>
#include "../mx_dll/mx_dll.h"
#include<vector>

struct foo_bar
{
	float qing;
	float wa;
};


void RenderDataCallback_Imp(void* data_ptr, int cnt)
{
	if (cnt = 0xffffffff)
	{

		return;
	}
	std::cout << "point count from callback " << cnt;
	std::vector<foo_bar> array;
	foo_bar* ptr =(foo_bar*)data_ptr;
	while (cnt-- > 0)
	{
		array.push_back(*ptr);
		ptr++;
	}	

	// 此处data_ptr泄露 unity端需要用Marshal释放
	std::cout << "callback finished!\n";
}


typedef  int  (*set_data_func)(char* string);

typedef void (*set_renderdata_callback_func)(RenderDataCallback callback);
int main()
{
	HMODULE  g_hDll = LoadLibrary(L"mx_dll.dll");
	if (g_hDll)
	{
		{
			set_renderdata_callback_func func = (set_renderdata_callback_func)GetProcAddress(g_hDll, "set_renderdata_callback");
			func(&RenderDataCallback_Imp);
		}

		{
			set_data_func func = (set_data_func)GetProcAddress(g_hDll, "set_data");

			const char* test_str = "F://yy//unity (2)//unity//Data";
			func((char*)test_str);
		}		
	}
	std::cout << "finished!\n";
}

