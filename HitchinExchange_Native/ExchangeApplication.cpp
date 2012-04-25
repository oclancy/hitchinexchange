#include "StdAfx.h"
#include "ExchangeApplication.h"


CExchangeApplication::CExchangeApplication(void)
{
}


CExchangeApplication::~CExchangeApplication(void)
{
}

/// Notification of a session begin created
void CExchangeApplication::onCreate( const SessionID& )
{
}

/// Notification of a session successfully logging on
void CExchangeApplication::onLogon( const SessionID& )
{
}

/// Notification of a session logging off or disconnecting
void CExchangeApplication::onLogout( const SessionID& )
{
}
	
/// Notification of admin message being sent to target
void CExchangeApplication::toAdmin( Message&, const SessionID& )
{
}
	
/// Notification of app message being sent to target
void CExchangeApplication::toApp( Message&, const SessionID& ) throw( DoNotSend )
{
}
	
/// Notification of admin message being received from target
void CExchangeApplication::fromAdmin( const Message&, const SessionID& ) throw( FieldNotFound, IncorrectDataFormat, IncorrectTagValue, RejectLogon )
{
}
	
/// Notification of app message being received from target
void CExchangeApplication::fromApp( const Message&, const SessionID& ) throw( FieldNotFound, IncorrectDataFormat, IncorrectTagValue, UnsupportedMessageType )
{
}


