package entity

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestCreateTransaction(t *testing.T) {
	client1, _ := NewClient("John Doe", "jhd@email.com")
	account1, _ := NewAccount(client1)
	client2, _ := NewClient("Jane Doe", "jnd@email.com")
	account2, _ := NewAccount(client2)

	account1.Credit(1000)
	account2.Credit(1000)

	transaction, err := NewTransaction(account1, account2, 100)
	assert.Nil(t, err)
	assert.NotNil(t, transaction)
	assert.Equal(t, 1100.0, account2.Balance)
	assert.Equal(t, 900.0, account1.Balance)
}

func TestCreateTransactionWithInsufficientBalance(t *testing.T) {
	client1, _ := NewClient("John Doe", "jhd@email.com")
	account1, _ := NewAccount(client1)
	client2, _ := NewClient("Jane Doe", "jnd@email.com")
	account2, _ := NewAccount(client2)

	account1.Credit(1000)
	account2.Credit(1000)

	transaction, err := NewTransaction(account1, account2, 2000)
	assert.NotNil(t, err)
	assert.Error(t, err, "insufficient funds")
	assert.Nil(t, transaction)
	assert.Equal(t, 1000.0, account2.Balance)
	assert.Equal(t, 1000.0, account1.Balance)
}

func TestCreateTransactionWithNegativeAmount(t *testing.T) {
	client1, _ := NewClient("John Doe", "jhd@email.com")
	account1, _ := NewAccount(client1)
	client2, _ := NewClient("Jane Doe", "jnd@email.com")
	account2, _ := NewAccount(client2)

	account1.Credit(1000)
	account2.Credit(1000)

	transaction, err := NewTransaction(account1, account2, -100)
	assert.NotNil(t, err)
	assert.Error(t, err, "amount must be greater than zero")
	assert.Nil(t, transaction)
	assert.Equal(t, 1000.0, account2.Balance)
	assert.Equal(t, 1000.0, account1.Balance)
}
