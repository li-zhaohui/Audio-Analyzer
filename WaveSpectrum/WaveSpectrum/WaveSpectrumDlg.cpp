
// WaveSpectrumDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "WaveSpectrum.h"
#include "WaveSpectrumDlg.h"
#include "afxdialogex.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CAboutDlg dialog used for App About

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CWaveSpectrumDlg dialog



CWaveSpectrumDlg::CWaveSpectrumDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_WAVESPECTRUM_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CWaveSpectrumDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CWaveSpectrumDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDOK, &CWaveSpectrumDlg::OnBnClickedOk)
END_MESSAGE_MAP()


// CWaveSpectrumDlg message handlers

BOOL CWaveSpectrumDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != nullptr)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here

	//ReadFile(_T("example.wav"));
	//GetSpectrum();

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CWaveSpectrumDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CWaveSpectrumDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CWaveSpectrumDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CWaveSpectrumDlg::OnBnClickedOk()
{
	// TODO: Add your control notification handler code here
	CFileDialog dlg(TRUE);
	if (dlg.DoModal() == IDOK) 
	{
		ReadFile(dlg.GetPathName());
		GetSpectrum();
	}
}

void CWaveSpectrumDlg::ReadFile(CString file_name)
{
	m_wave_reader.SetFile(file_name);
	if (m_wave_reader.is_valid == false)
	{
		AfxMessageBox(_T("Invalid Wave Audio File"));
		return;
	}

	CString str;

	str.Format(_T("%d"), m_wave_reader.samplerate);
	GetDlgItem(IDC_SAMPLE_RATE)->SetWindowText(str);

	str.Format(_T("%d"), m_wave_reader.duration);
	GetDlgItem(IDC_DURATION)->SetWindowText(str);

}

void CWaveSpectrumDlg::GetSpectrum()
{
	if (m_wave_reader.is_valid == FALSE) 
	{
		return;
	}
	//int wnd_size = sqrt(m_wave_reader.samplecount);
	int wnd_size = m_wave_reader.samplerate * 0.02;
	m_dct.SetWindowSize(wnd_size);

	int tick_count = (m_wave_reader.samplecount - 1) / wnd_size;
	tick_count++;
	float** spectre = new float* [tick_count];

	int sample_offset = 0;
	for (int i = 0; i < tick_count; i++)
	{
		spectre[i] = new float[wnd_size]; 
		
		int in_count = m_wave_reader.samplecount - sample_offset;
		if(in_count > wnd_size)
		{
			in_count = wnd_size;
		}
		
		m_dct.compute(m_wave_reader.sampledata + sample_offset, in_count, spectre[i]);

		sample_offset += wnd_size;
	}

	CImage img;
	img.Create(tick_count, wnd_size, 32);
	
	COLORREF rgb;
	for (int x = 0; x < tick_count; x++) 
	{
		for (int y = 0; y < wnd_size; y++) 
		{
			int red = abs(spectre[x][y]) * 255;
			red = max(min(red, 255), 0);
			rgb = RGB(red, 0,0);
			img.SetPixel(x, wnd_size - y - 1, rgb);
		}
	}
	img.Save(m_wave_reader.m_strFileName+_T(".PNG"));
	
	ShellExecute(this->m_hWnd, NULL, m_wave_reader.m_strFileName + _T(".PNG"), NULL, NULL, SW_SHOW);
}