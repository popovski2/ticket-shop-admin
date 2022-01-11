describe('Admin application E2E testing', () => {
    beforeEach(() => {
        // Cypress starts out with a blank slate for each test
        // so we must tell it to visit our website with the `cy.visit()` command.
        // Since we want to visit the same URL at the start of all our tests,
        // we include it in our beforeEach function so that it runs before each test
        cy.visit('https://localhost:44398/')
    })

    

    it('should have export all orders button', () => {

        cy.get("#all-orders").click();
       // cy.get("#export-orders").click();
        //assert
        cy.get("#export-orders").should('have.text', 'Export Orders');

        

    })

    it('should have get invoice button', () => {

        cy.get("#all-orders").click();
        
        //assert
        cy.get("#get-invoice").should('have.text', 'Get Invoice');



    })

    it('should have view order ', () => {

        cy.get("#all-orders").click();
        cy.get("#view-order").click(); //opens details
        
        //assert
        cy.get("#order-for").contains("Order for:");
        



    })

    it('clicks view order', () => {

        cy.get("#all-orders").click();
        cy.get("#view-order").click();
        //assert

        cy.get("#order-for").contains("Order for:");
        
        
     

    })

    

    //import users
    it('clicks import users', () => {

        cy.get("#import-users").click();

        //assert
        cy.wait(1000);
        cy.get("#import-excel").should('have.text', 'Import Excel');


    })


    //DOWNLOAD EXCEL
    it('should download excel orders [CYPRESS BUG - INFINITE PAGE LOAD WHEN DOWNLOADING]', () => {

        cy.get("#all-orders").click();
        cy.get("#export-orders").click();
        //assert
        cy.get("#export-orders").should('have.text', 'Export Orders');



    })

    //GET INVOICE
    it('should download pdf invoice [CYPRESS BUG - INFINITE PAGE LOAD WHEN DOWNLOADING]', () => {

        cy.get("#all-orders").click();
        cy.get("#get-invoice").click();
        //assert
        cy.get("#get-invoice").should('have.text', 'Get Invoice');



    })








})
