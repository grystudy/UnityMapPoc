#pragma once
#ifdef MY_DLL_EXPORTS
#define MY_DLL_API __declspec(dllexport)
#else
#define MY_DLL_API __declspec(dllimport)
#endif


extern "C" MY_DLL_API int  set_data(char* data_path);

extern "C"
{
    typedef void (*RenderDataCallback)(void* data_ptr, int cnt);
    
    MY_DLL_API void set_renderdata_callback(RenderDataCallback callback);
}