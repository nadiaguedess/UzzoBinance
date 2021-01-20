use UzzoBinance;

CREATE TABLE SymbolPrice (
    idSymbolPrice int IDENTITY(1,1) PRIMARY KEY,
    symbol varchar(20) NOT NULL,
    bidPrice decimal(21,9),
    askPrice decimal(21,9),
	dateAndTimePrice datetime
);
