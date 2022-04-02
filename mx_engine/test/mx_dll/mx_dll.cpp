#include <iostream>
#include <string>

#include "mx_dll.h"

#include <windows.h>
#include "cjson/cJSON.h"

#include<vector>

#include "../test/test.h"

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
typedef int (*db_parser_get_routing_shape_points_serialize)(int tile_id, byte* road_shape_points);
struct coordinate
{
	float x;
	float y;
};

struct mesh
{
	Vector3* vertices;
	int vertices_cnt;

	int* indexes;
	int index_cnt;

	mesh()
	{
		vertices = new Vector3[250]();
		vertices_cnt = 250;

		//memset(vertices, 1, sizeof(Vector3) * vertices_cnt);

		for (int i = 0; i < vertices_cnt; ++i)
		{
			auto& a = vertices[i];
			a.x = 1;
			a.y = 2;
			a.z = 3;
		}

		indexes = new int[750]();
		index_cnt = 750;
		memset(indexes, 10, sizeof(int) * index_cnt);
	}
};

struct background
{
	mesh* green;
	int gnreen_cnt;

	mesh* water;
	int water_cnt;
};

struct tile
{
	Vector2 coor;
	background a;
	background b;
};

RenderDataCallback _myCallback;
extern "C"
{
	void set_renderdata_callback(RenderDataCallback callback) {
		_myCallback = callback;
	}

	MY_DLL_API int set_data(char* data_path)
	{
		std::string path = data_path;
		std::wstring dll_path = std::wstring(path.begin(), path.end()) + L"\\nds_ds.dll";
		HMODULE  g_hDll = LoadLibraryEx(dll_path.c_str(), NULL, LOAD_WITH_ALTERED_SEARCH_PATH);
		if (g_hDll)
		{
			{
				std::string path = data_path;
				std::string log_path = path + "\\log2022\\";
				db_parser_init func = (db_parser_init)GetProcAddress(g_hDll, "db_parser_init");
				auto res = func(path.c_str(), log_path.c_str(), "1100");

				std::cout << "db_parser_init:  " << res << std::endl;
				if (res != 0)
					return 250;
			}

			{
				const size_t size = 50000000;
				thread_local byte* data = new byte[size]();
				memset(data, 0, size);
				db_parser_get_routing_shape_points_serialize func = (db_parser_get_routing_shape_points_serialize)GetProcAddress(g_hDll, "db_parser_get_routing_shape_points_serialize");
				auto res = func(557467546, data);
				if (res != 0)
					return 251;

				//#define cJSON_False 0
				//#define cJSON_True 1
				//#define cJSON_NULL 2
				//#define cJSON_Number 3
				//#define cJSON_String 4
				//#define cJSON_Array 5
				//#define cJSON_Object 6
				//
				//#define cJSON_IsReference 256

								/*{
									"coordinates": [
										[
										{
											"x":116.47691666666667,
												"y" : 39.98085069444444
										},
							{
								"x":116.47611805555556,
								"y" : 39.981378472222225
							}
										],
											[
											{
												"x":116.46579166666666,
													"y" : 39.988197916666664
											},
							{
								"x":116.46588194444445,
								"y" : 39.98828819444444
							}
											],*/

											/*thread_local std::vector<coordinate> shapes;
											shapes.clear();*/

				cJSON* root = cJSON_Parse((const char*)data);
				if (!root)
					return 252;

				cJSON* cur = root->child->child;

				int cnt = root->child->next->valueint;

				coordinate* shapes = new coordinate[cnt]();
				int i = 0;

				coordinate coor_p = { 0 };
				while (cur)
				{
					switch (cur->type)
					{
					case cJSON_Array:
					{
						if (cur->child)
						{
							cJSON* mv_file_item = cur->child->child;
							while (mv_file_item)
							{
								if ((strcmp("x", mv_file_item->string) == 0))
								{
									coor_p.x = (float)mv_file_item->valuedouble;
								}
								else
								{
									coor_p.y = (float)mv_file_item->valuedouble;

									shapes[i++] = (coor_p);
								}
								mv_file_item = mv_file_item->next;
							}
						}
					}
					}
					cur = cur->next;
				}

				std::cout << "point cnt: " << cnt << std::endl;
				if (_myCallback)
				{
					_myCallback(shapes, cnt);
				}
			}

			{
				Vector2 v2 = { 1,11 };
				Vector3 v3 = { 250.0f,251.0f,252.0f };

				tile* t11 = new tile();
				tile& t1 = *t11;
				t1.coor = v2;

				auto cnt = 100;
				t1.a.green = new mesh[cnt]();
				t1.a.gnreen_cnt = cnt;

				t1.a.water = new mesh[cnt]();
				t1.a.water_cnt = cnt;


				t1.b.green = new mesh[cnt]();
				t1.b.gnreen_cnt = cnt;

				t1.b.water = new mesh[cnt]();
				t1.b.water_cnt = cnt;
				if (_myCallback)
				{
					_myCallback(t11, 0xffffffff);
				}
			}
		}


		std::cout << data_path << std::endl;
		return 0;
	}
}