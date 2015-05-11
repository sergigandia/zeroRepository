// ModTextureGtaSa.h : main header file for the ModTextureGtaSa DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CModTextureGtaSaApp
// See ModTextureGtaSa.cpp for the implementation of this class
//

class CModTextureGtaSaApp : public CWinApp
{
public:
	CModTextureGtaSaApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
