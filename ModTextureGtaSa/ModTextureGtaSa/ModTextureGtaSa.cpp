#ifndef _DLL_H_
#define _DLL_H_

#if BUILDING_DLL
# define DLLIMPORT __declspec (dllexport)
#else /* Not BUILDING_DLL */
# define DLLIMPORT __declspec (dllimport)
#endif /* Not BUILDING_DLL */

#include "plugin\plugin.h"
#include "game_sa\common.h"
#include "game_sa\CTimer.h"
#include "D3D9Headers\d3dx9tex.h"

using namespace plugin;

BOOL APIENTRY DllMain(HMODULE module, DWORD reason, LPVOID reserved)
{
	if (reason == DLL_PROCESS_ATTACH)
		Core::RegisterFunc(FUNC_GAME_PROCESS_AFTER_SCRIPTS, MyProcess);
	return TRUE;
}

void MyProcess()
{
	D3DXCreateTexture(, );
}

#endif /* _DLL_H_ */