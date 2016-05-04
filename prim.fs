\ This is a prime calculator program

: setup ( n -- a-addr n )
  cells allocate throw
  2 over !	\ store 2 in the beginning of the sieve
  1 \ swap 	\ one element in the sieve
;

: teardown ( a-addr -- )
  free throw
;

: checksieve ( a-addr n1 n2 -- n1 f ) recursive \ checks if n1 is a multiple of the n2 primes already stored in a-addr
dup 0 > if
  swap rot over \ primes candidate address candidate
  over @   	\ copy address to top and read
  mod 		\ push modulo calculation onto stack
  0 > if	\ if != 0
    1 cells +	\ calculate offset address for recursive call
    rot 1 - 	\ decrement amount of primes to check
    rot swap	\ restore order
    checksieve  \ recursive call
  else
     rot drop drop true \ clean up after ourselves
  endif
else
   over dec.
   rot drop drop false \ clean up after ourselves
endif
;

: copy3 { a b c -- a b c a b c }
\ over 2over drop 2over nip rot swap
a b c a b c
;


: run ( a-addr n1 n2 -- ) recursive \ n1 number of cells in a-addr, n2 number of already occupied cells
2dup <> if
   rot
   2dup
   2dup
   2dup \ rearrange make copies of address and cell number
   swap
   1 -
   cells + @

   1 +	\ generate candidate from last occupied cell
   rot
   checksieve
   0= if
      swap rot		\ arrange arguments so we can store in the cell
      cells + !		\ store
      swap 1 + swap	\ increment number of found primes
      rot rot  		\ rearrange arguments
      run 		\ recursive call
   else
      -1
      begin
        0 <> while
        1 +
        1 2over 2tuck 2drop drop
        rot
        checksieve
      repeat
      swap rot
      cells + !		\ store
      swap 1 + swap	\ increment number of found primes
      rot rot  		\ rearrange arguments
      run 		\ recursive call
   endif

endif
;

clearstack 10000 setup 10000 swap run

\ clearstack 10 setup 
\ drop dup dup
\ 3 tuck drop 1 cells + ! 5 tuck drop 2 cells + !
\ 9 3 checksieve



