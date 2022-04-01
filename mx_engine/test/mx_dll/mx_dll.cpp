#include <iostream>
#include <string>

#include "mx_dll.h"

#include <windows.h>


//[DllImport("nds_ds")]
//extern static int db_parser_init(string db_root_path, string log_path, string citycode);
//[DllImport("nds_ds")]
//extern static int db_parser_uint();
//[DllImport("nds_ds")]
//extern static int db_parser_get_tile_Id(float x, float y, int level, ref int tile_id_);
//[DllImport("nds_ds")]
//extern static int db_parser_get_tile_Ids_serialize(float ax, float ay, float bx, float by, int level, ref byte tile_ids);
//[DllImport("nds_ds")]
//extern static int db_parser_get_routing_shape_points_serialize(int tile_id, ref byte road_shape_points);
//
//[DllImport("nds_ds")]
//extern static int db_parser_get_tile_rect_range(int tile_id, ref float tile_ax, ref float tile_ay, ref float tile_bx, ref float tile_by);
//[DllImport("nds_ds")]
//extern static int db_parser_get_building_serialize(int tile_id, ref byte building);
//[DllImport("nds_ds")]
//extern static int db_parser_get_lower_tile_id_serialize(int tile_id, int level, ref byte tileIdList);
//
//[DllImport("nds_ds")]
//extern static int db_parser_get_water_system_serialize(int tile_id, ref byte areas);
//[DllImport("nds_ds")]
//extern static int db_parser_get_vegetated_area_serialize(int tile_id, ref byte areas);


typedef int (*db_parser_init)(const char* db_root_path, const char* log_path, const char* citycode);
extern "C"
{
	
	MY_DLL_API int set_data(char* data_path)
	{
		HMODULE  g_hDll = LoadLibrary(L"nds_ds.dll");
		if (g_hDll)
		{
			std::string path = data_path;
			std::string log_path = path + "\\log2022\\";
			db_parser_init func = (db_parser_init)GetProcAddress(g_hDll, "db_parser_init");
			auto res = func(path.c_str(), log_path.c_str(), "1100");

			std::cout << "db_parser_init:  " << res << std::endl;
		}


		std::cout << data_path << std::endl;
		return 100;
	}
}