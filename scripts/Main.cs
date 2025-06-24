using System;
using Godot;


public partial class Main : Node2D{
	int[,] field;
	int resolution = 10;
	int screen_width = 1520;
	int screen_hight = 780;
	int rows, cols;
	float z_off;
	FastNoiseLite noise;
	float increment = 0.05f;
	public override void _Ready(){
		
		GetWindow().Size = new Vector2I(screen_width, screen_hight);
		DisplayServer.WindowSetPosition(new Vector2I(0, 100));
		rows = 1 + screen_hight / resolution;
		cols = 1 + screen_width / resolution;
		field = new int[rows, cols];

		noise = new FastNoiseLite();
		noise.NoiseType = FastNoiseLite.NoiseTypeEnum.Perlin;
		noise.Frequency = 1;
	}

	public override void _Process(double delta) {
		float x_off = 0;
		for (int i = 0; i < cols; i++) {
			x_off += increment;
			float y_off = 0;
			for (int j = 0; j < rows; j++) {
				float noiseValue = noise.GetNoise3D(x_off, y_off, z_off); // Range: [-1, 1]
				field[j, i] = noiseValue > 0 ? 1 : 0;

				y_off += increment;
			}
		}
		z_off += 0.003f;
		QueueRedraw();
	}


    public override void _Draw() {

		// draw circles to represent noise values ( 0 or 1)
		// for (int i = 0; i < cols; i++) {
		// 	for (int j = 0; j < rows; j++) {
		// 		float x = i * resolution;
		// 		float y = j * resolution;
				
		// 		if (field[j, i] > 0) {
		// 			DrawCircle(new Vector2(x, y), 5, Colors.Black);
		// 		} else {
		// 			DrawCircle(new Vector2(x, y), 5, Colors.White);
		// 		}
		// 	}
		// }

		for (int i = 0; i < cols - 1; i++) {
			for (int j = 0; j < rows - 1; j++) {
				float x = i * resolution;
				float y = j * resolution;

				Vector2 a = new Vector2(x + 0.5f * resolution, y);
				Vector2 b = new Vector2(x + resolution, y + 0.5f * resolution);
				Vector2 c = new Vector2(x + 0.5f * resolution, y + resolution);
				Vector2 d = new Vector2(x, y + 0.5f * resolution);

				int state = field[j, i] * 8
						+ field[j, i + 1] * 4
						+ field[j + 1, i + 1] * 2
						+ field[j + 1, i];

				switch (state) {
					case 1: DrawLine(c, d, Colors.Black); break;
					case 2: DrawLine(b, c, Colors.Black); break;
					case 3: DrawLine(d, b, Colors.Black); break;
					case 4: DrawLine(a, b, Colors.Black); break;
					case 5:
						DrawLine(a, d, Colors.Black);
						DrawLine(b, c, Colors.Black);
						break;
					case 6: DrawLine(a, c, Colors.Black); break;
					case 7: DrawLine(a, d, Colors.Black); break;
					case 8: DrawLine(a, d, Colors.Black); break;
					case 9: DrawLine(a, c, Colors.Black); break;
					case 10:
						DrawLine(a, b, Colors.Black);
						DrawLine(c, d, Colors.Black);
						break;
					case 11: DrawLine(a, b, Colors.Black); break;
					case 12: DrawLine(b, d, Colors.Black); break;
					case 13: DrawLine(b, c, Colors.Black); break;
					case 14: DrawLine(c, d, Colors.Black); break;
				}
			}
		}
	}
}
