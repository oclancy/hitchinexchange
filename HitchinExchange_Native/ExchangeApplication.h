#pragma once

#include "quickfix\Application.h"
#include "quickfix\SessionID.h"
#include "quickfix\Message.h"

using namespace FIX;

class CExchangeApplication : public Application
{
public:
	CExchangeApplication(void);
	~CExchangeApplication(void);

	/// Notification of a session begin created
	virtual void onCreate( const SessionID& );

	/// Notification of a session successfully logging on
	virtual void onLogon( const SessionID& );

	/// Notification of a session logging off or disconnecting
	virtual void onLogout( const SessionID& );
	
	/// Notification of admin message being sent to target
	virtual void toAdmin( Message&, const SessionID& );
	
	/// Notification of app message being sent to target
	virtual void toApp( Message&, const SessionID& ) throw( DoNotSend );
	
	/// Notification of admin message being received from target
	virtual void fromAdmin( const Message&, const SessionID& ) throw( FieldNotFound, IncorrectDataFormat, IncorrectTagValue, RejectLogon );
	
	/// Notification of app message being received from target
	virtual void fromApp( const Message&, const SessionID& ) throw( FieldNotFound, IncorrectDataFormat, IncorrectTagValue, UnsupportedMessageType );
};

