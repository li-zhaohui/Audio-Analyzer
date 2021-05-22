
// WaveSpectrumDlg.h : header file
//

#pragma once
#include "WaveReader.h"
#include "DCT.h"
// CWaveSpectrumDlg dialog
class CWaveSpectrumDlg : public CDialogEx
{
// Construction
public:
	CWaveSpectrumDlg(CWnd* pParent = nullptr);	// standard constructor

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_WAVESPECTRUM_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	WaveReader m_wave_reader;
	DCT			m_dct;

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();

	void ReadFile(CString file_name);
	void GetSpectrum();

};
