#include <vector>

struct Vector3 {
    float x{0};
    float y{0};
    float z{0};
};

struct Vector2 {
    float x{0};
    float y{0};
};

struct Mesh {
    std::vector<Vector3> vertices;
    std::vector<int> indexes;
    std::vector<Vector3> normals;
    std::vector<Vector2> uvs;
};

struct RoadList {
    std::vector<Mesh> road_level_1;
    std::vector<Mesh> road_level_2;
    std::vector<Mesh> road_level_3;
};

struct BackgroundList {
    std::vector<Mesh> green_land;
    std::vector<Mesh> water;
};

struct BuildingList {
    std::vector<Mesh> buildings;
};

struct Tile {
    Vector2 coordinate;
    RoadList roads;
    BackgroundList backgrounds;
    BuildingList buildings;
};

// std::vector<Tile> tiles;