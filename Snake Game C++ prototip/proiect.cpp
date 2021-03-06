#include "stdafx.h"
#include <iostream>
#include <conio.h>
#include <windows.h>
using namespace std;
bool sfarsitulJocului;
const int latime = 50;
const int inaltime = 20;
int x, y, fructX, fructY, punctaj;
int coadaX[50], coadaY[50];
int nrCoada;
int level = 1;
enum directie { STOP = 0, LEFT, RIGHT, UP, DOWN};
directie dir;
void Structura()
{
	sfarsitulJocului = false;
	dir = STOP;
	x = latime / 2;
	y = inaltime / 2;
	fructX = rand() % latime;
	fructY = rand() % inaltime;
	punctaj = 0;
}
void Grafica()
{
	system("cls");
	for (int i = 0; i < latime+2; i++)
		cout << "X";
	cout << endl;

	for (int i = 0; i < inaltime; i++)
	{
		for (int j = 0; j < latime; j++)
		{
			if (j == 0)
				cout << "X";
			if (i == y && j == x)
				cout << "O";
			else if (i == fructY && j == fructX)
				cout << "F";
			else
			{
				bool afisare = false;
				for (int k = 0; k < nrCoada; k++)
				{
					if (coadaX[k] == j && coadaY[k] == i)
					{
						cout << "o";
						afisare = true;
					}
				}
				if (!afisare)
				cout << " ";
			}
			if (j == latime - 1)
				cout << "X";
		}
		cout << endl;
	}

	for (int i = 0; i < latime+2; i++)
		cout << "X";
	cout << endl;
	cout << "Punctaj:" << punctaj << endl;
	cout << "Level:" << level << endl;
//	if (sfarsitulJocului == true)
//		cout << "Te-ai muscat singur! Ai pierdut." << endl;
}
void Controale()
{
	if (_kbhit())
	{
		switch (_getch())
		{
		case 'a':
			dir = LEFT;
			break;
		case 'd':
			dir = RIGHT;
			break;
		case 'w':
			dir = UP;
			break;
		case 's':
			dir = DOWN;
			break;
		case 'q':
			sfarsitulJocului = true;
			break;
		}
	}
}
int cLevel()
{
	int v[10] = { 0,5,10,13,16,19,21,23,25,27 };
	int w = 0;
	if (w < 11)
		for (w = 0; w <= 10; w++)
			if (v[w] == punctaj)
				level = w + 1;
	return level;

//	if (punctaj == level*5)
//		level++;
}
void Logica(int &i, int& level )
{
	int antX = coadaX[0];
	int antY = coadaY[0];
	int ant2X, ant2Y;
	coadaX[0] = x;
	coadaY[0] = y;
	for (int i = 1; i < nrCoada; i++)
	{
		std::swap(coadaX[i], antX);
		std::swap(coadaY[i], antY);
		/*ant2X = coadaX[i];
		ant2Y = coadaY[i];
		coadaX[i] = antX;
		coadaY[i] = antY;
		antX = ant2X;
		antY = ant2Y;*/
	}
	switch (dir)
	{
	case STOP:
		break;
	case LEFT:
		x--;
		break;		
	case RIGHT:
		x++;
		break;
	case UP:
		y--;
		break;
	case DOWN:
		y++;
		break;
	default:
		break;
	}
	//if (x > latime || x < 0 || y > inaltime || y < 0)
		//sfarsitulJocului = true;
	if (x >= latime) x = 0;
	else if (x < 0)
		x = latime - 1;
	if (y >= inaltime) y = 0;
	else if (y < 0)
		y = inaltime - 1;
	for (int i = 0; i < nrCoada; i++)
		if (coadaX[i] == x && coadaY[i] == y)
			sfarsitulJocului = true;
	if (x == fructX && y == fructY)
	{
		punctaj += 1;
		if (i > 10)
		{
			i = i - 10;
		}
		fructX = rand() % latime;
		fructY = rand() % inaltime;
		nrCoada++;
		level = cLevel();
	}
}
int main()
{
	int i = 100;
	int level = 1;
	Structura();
	while (!sfarsitulJocului)
	{
		Grafica();
		Controale();
		Logica(i, level);
		Sleep(i+1);

		if (level == 10)
		{
			cout << "Ai castigat!" << endl;
			sfarsitulJocului = true;
			system("pause");
		}
	}
	if (level < 2)
	{
		cout << "Ai pierdut!" << endl;
		system("pause");
	}
	return 0;
}
