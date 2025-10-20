package createtransaction

import (
	"testing"

	"github.com/raffreitas/fc-ms-wallet/internal/entity"
	"github.com/stretchr/testify/assert"
	"github.com/stretchr/testify/mock"
)

type TransactionGatewayMock struct {
	mock.Mock
}

type AccountGatewayMock struct {
	mock.Mock
}

func (m *TransactionGatewayMock) Create(transaction *entity.Transaction) error {
	args := m.Called(transaction)
	return args.Error(0)
}

func (m *AccountGatewayMock) Save(account *entity.Account) error {
	args := m.Called(account)
	return args.Error(0)
}

func (m *AccountGatewayMock) FindByID(id string) (*entity.Account, error) {
	args := m.Called(id)
	return args.Get(0).(*entity.Account), args.Error(1)
}

func TestCreateAccountUseCase_Execute(t *testing.T) {
	clientFrom, _ := entity.NewClient("John Doe", "jhd@email.com")
	accountFrom, _ := entity.NewAccount(clientFrom)
	accountFrom.Credit(1000)

	clientTo, _ := entity.NewClient("Jane Doe", "jnd@email.com")
	accountTo, _ := entity.NewAccount(clientTo)
	accountTo.Credit(1000)

	accountMock := &AccountGatewayMock{}
	accountMock.On("FindByID", accountFrom.ID).Return(accountFrom, nil)
	accountMock.On("FindByID", accountTo.ID).Return(accountTo, nil)

	transactionMock := &TransactionGatewayMock{}
	transactionMock.On("Create", mock.Anything).Return(nil)

	uc := NewCreateTransactionUseCase(transactionMock, accountMock)
	inputDto := &CreateTransactionInputDTO{
		AccountIDFrom: accountFrom.ID,
		AccountIDTo:   accountTo.ID,
		Amount:        100,
	}

	output, err := uc.Execute(inputDto)

	assert.Nil(t, err)
	assert.NotNil(t, output)
	assert.NotEmpty(t, output.ID)

	accountMock.AssertExpectations(t)
	transactionMock.AssertExpectations(t)
	accountMock.AssertNumberOfCalls(t, "FindByID", 2)
	transactionMock.AssertNumberOfCalls(t, "Create", 1)
}
